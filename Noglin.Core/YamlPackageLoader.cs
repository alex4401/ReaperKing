using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using SharpYaml;
using SharpYaml.Events;
using SharpYaml.Serialization;

using Xeno.Core;

namespace Noglin.Core
{
    public class YamlPackageLoader : IPackageLoader
    {
        private struct SchemaDescriptor
        {
            public string SchemaName { get; init; }
            public Type SchemaType { get; init; }
        }
        
        private static readonly CamelCaseNamingConvention NamingConvention = new();
        
        private ILogger Log { get; init; }
        private readonly List<SchemaDescriptor> _types = new();

        public YamlPackageLoader(ILoggerFactory factory)
        {
            Log = factory.CreateLogger<YamlPackageLoader>();
        }

        public void AddType(Type type)
        {
            // Retrieve the project property attribute.
            var attributes = type.GetCustomAttributes<NoglinYamlAttribute>().ToArray();

            if (attributes.Length < 1)
            {
                throw new ArgumentException($"{type.FullName} does not have an NoglinSchema " +
                                            "attribute and thus cannot be exposed to project configurations.");
            }

            foreach (NoglinYamlAttribute attribute in attributes)
            {
                // Push a descriptor of the property T.
                _types.Add(new SchemaDescriptor
                {
                    SchemaName = attribute.Name,
                    SchemaType = type,
                });
            }
        }

        public void AddType<T>()
        {
            AddType(typeof(T));
        }

        public object LoadFile(string path)
        {
            using (StringReader reader = new(File.ReadAllText(path)))
            {
                Parser parser = new Parser(reader);
                EventReader eventStream = new EventReader(parser);

                eventStream.Expect<StreamStart>();
                eventStream.Expect<DocumentStart>();
                eventStream.Expect<MappingStart>();

                Type schemaType = CheckFileSchemaType(eventStream);
                if (schemaType == null)
                {
                    throw new SerializationException($"Failed to determine the data type of package \"{path}\".");
                }

                return ParsingUtils.ReadYamlFile(path, Activator.CreateInstance(schemaType));
            }
        }

        public T LoadFile<T>(string path)
            where T : class
        {
            return LoadFile(path) as T;
        }

        private Type CheckFileSchemaType(EventReader eventStream)
        {
            Scalar key = eventStream.Expect<Scalar>();
            Scalar value = eventStream.Expect<Scalar>();

            if (key.Value != "apiVersion")
            {
                Log.LogCritical("Tried to load a file as a data package, but its schema is not specified.");
                LogKnownSchemas();
                return null;
            }

            SchemaDescriptor[] matches = _types.Where(
                info => info.SchemaName == value.Value
            ).ToArray();

            if (matches.Length < 1)
            {
                Log.LogCritical("Tried to load a file as a data package, but its schema \""
                               + value.Value + "\" is not known.");
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
                Log.LogCritical("- " + info.SchemaName);
            }
        }
    }
}