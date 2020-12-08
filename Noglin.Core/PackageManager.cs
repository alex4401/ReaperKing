using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using ReaperKing.Core;

namespace Noglin.Core
{
    public class PackageManager
    {
        protected ILogger Log;
        private PackageTree _tree = new PackageTree("@");
        private Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public PackageManager(ILogger log)
        {
            Log = log;
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName != null
                    && (assembly.FullName.StartsWith("Noglin") || assembly.FullName.StartsWith("ReaperKing")))
                {
                    ScanAssembly(assembly);
                }
            }
        }

        public void ScanAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttribute<PackageTypeAttribute>();
                if (attribute == null)
                {
                    continue;
                }

                _types[attribute.PublicTypeName] = type;
            }
        }
        
        public void IngestFile(string filename)
        {
            dynamic temp = ParsingUtils.ReadYamlFile<dynamic>(filename);

            if (!temp.GetType().GetProperty("type"))
            {
                return;
            }

            string typeName = temp.type;
        }
    }
}