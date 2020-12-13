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

using ReaperKing.Core;

namespace Noglin.Core
{
    public class UnifiedPackageLoader : IPackageLoader
    {
        private ILogger Log { get; init; }
        private YamlPackageLoader YamlLoader { get; init; }
        private JsonPackageLoader JsonLoader { get; init; }

        public UnifiedPackageLoader(ILoggerFactory factory)
        {
            Log = factory.CreateLogger<UnifiedPackageLoader>();
            YamlLoader = new(factory);
            JsonLoader = new(factory);
        }

        public void AddType(Type type)
        {
            if (type.GetCustomAttributes<NoglinYamlAttribute>().Any())
            {
                YamlLoader.AddType(type);
            }
            
            if (type.GetCustomAttributes<NoglinJsonAttribute>().Any())
            {
                JsonLoader.AddType(type);
            }
        }

        public void AddType<T>()
        {
            AddType(typeof(T));
        }

        public object LoadFile(string path)
        {
            switch (Path.GetExtension(path))
            {
                case ".json":
                    return JsonLoader.LoadFile(path);
                
                case ".yaml":
                    return YamlLoader.LoadFile(path);
                
                default:
                    throw new ArgumentException($"\"{path}\" is not a YAML or JSON package.");
            }
        }

        public T LoadFile<T>(string path)
            where T : class
        {
            return LoadFile(path) as T;
        }
    }
}