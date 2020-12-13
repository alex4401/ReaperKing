using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using SharpYaml;
using SharpYaml.Events;
using SharpYaml.Serialization;

using ReaperKing.Core;

namespace Noglin.Core
{
    public class JsonPackageLoader : IPackageLoader
    {
        private const int MaxPropertyReadCount = 5;
        public static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        
        private struct SchemaDescriptor
        {
            public string SchemaName { get; init; }
            public Type SchemaType { get; init; }
            public int SchemaVersion { get; init; }
        }
        
        private ILogger Log { get; init; }
        private readonly List<SchemaDescriptor> _types = new();

        public JsonPackageLoader(ILoggerFactory factory)
        {
            Log = factory.CreateLogger<JsonPackageLoader>();
        }

        public void AddType(Type type)
        {
            // Retrieve the project property attribute.
            var attributes = type.GetCustomAttributes<NoglinJsonAttribute>().ToArray();

            if (attributes.Length < 1)
            {
                throw new ArgumentException($"{type.FullName} does not have a NoglinJson " +
                                            "attribute and thus cannot be exposed to project configurations.");
            }

            foreach (NoglinJsonAttribute attribute in attributes)
            {
                // Push a descriptor of the property T.
                _types.Add(new SchemaDescriptor
                {
                    SchemaName = Path.GetFileNameWithoutExtension(attribute.Name),
                    SchemaType = type,
                    SchemaVersion = attribute.Version,
                });
            }
        }

        public void AddType<T>()
        {
            AddType(typeof(T));
        }

        public object LoadFile(string path)
        {
            byte[] fileContents = File.ReadAllBytes(path);
            Utf8JsonReader reader = new(fileContents);
            reader.SkipNull();
            reader.AssertIs(JsonTokenType.StartObject);

            Type schemaType = CheckFileSchemaType(ref reader);
            if (schemaType == null)
            {
                throw new SerializationException($"Failed to determine the data type of package \"{path}\".");
            }
            
            return JsonSerializer.Deserialize(fileContents, schemaType, JsonOptions);
        }

        public T LoadFile<T>(string path)
            where T : class
        {
            return LoadFile(path) as T;
        }

        private Type CheckFileSchemaType(ref Utf8JsonReader reader)
        {
            string schemaName = "";
            int schemaVersion = -1;
            int readCount = 0;
            
            while (readCount++ < MaxPropertyReadCount
                   && (schemaName == "" || schemaVersion == -1))
            {
                reader.Expect(JsonTokenType.PropertyName);
                string propertyName = reader.GetString();

                switch (propertyName)
                {
                    case "$schema":
                        reader.Expect(JsonTokenType.String);
                        schemaName = Path.GetFileNameWithoutExtension(reader.GetString());
                        break;
                    
                    case "format":
                        reader.Expect(JsonTokenType.String);
                        if (!Int32.TryParse(reader.GetString(), out schemaVersion))
                        {
                            throw new InvalidDataException("Schema version found in the file is not valid.");
                        }
                        break;
                    
                    default:
                        reader.Skip();
                        continue;
                }
            }

            SchemaDescriptor[] matches = _types.Where(
                info => info.SchemaName == schemaName && info.SchemaVersion == schemaVersion
            ).ToArray();

            if (matches.Length < 1)
            {
                Log.LogCritical("Tried to load a file as a data package, but its schema " + 
                                $"\"{schemaName}:{schemaVersion}\" is not known.");
                LogKnownSchemas();
                return null;
            }

            return matches[0].SchemaType;
        }

        private void LogKnownSchemas()
        {
            Log.LogCritical("Known schemas:");

            foreach (SchemaDescriptor info in _types)
            {
                Log.LogCritical("- " + info.SchemaName + ":" + info.SchemaVersion);
            }
        }
    }
}