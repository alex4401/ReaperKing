/*!
 * This file is a part of Reaper King, and the project's repository may be found at
 * https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
 * version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using SharpYaml.Serialization;

using PropertyDict = System.Collections.Generic.Dictionary<string, System.Type>;

namespace ReaperKing.Core.Configuration
{
    public class SchemaCollectionManager
    {
        private static readonly CamelCaseNamingConvention NamingConvention = new();
        
        internal class SchemaInfo
        {
            public string Super { get; internal set; }
            public PropertyDict Properties { get; } = new();
            public Dictionary<string, PropertyDict> PropertySets { get; } = new();
        }

        private Dictionary<string, SchemaInfo> SchemaInfos { get; } = new();
        
        public SchemaCollectionManager()
        {
            ImportFromAssembly(typeof(SchemaCollectionManager).Assembly);
        }
        
        internal SchemaInfo this[string id]
            => SchemaInfos[id];

        public IEnumerable<string> Names
            => SchemaInfos.Keys;
        
        public bool IsValid(string id)
        {
            return Names.Contains(id);
        }
        
        public void ImportFromAssembly(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsVisible || !type.IsClass)
                {
                    continue;
                }
                
                RkSchemaAttribute attribute = type.GetCustomAttribute<RkSchemaAttribute>();
                if (attribute == null || !type.IsSubclassOf(typeof(Schema)))
                {
                    continue;
                }
                
                Register(type);
            }
        }
        
        public string Register<T>()
            where T : new()
            => Register(typeof(T));

        public string Register(Type schemaType)
        {
            RkSchemaAttribute attribute = schemaType.GetCustomAttribute<RkSchemaAttribute>();
            Debug.Assert(attribute != null, nameof(attribute) + " != null");
            Register(attribute.Identifier, schemaType);
            return attribute.Identifier;
        }

        /**
         * Registers a schema type if its identifier is not known.
         *
         * It is possible that registering schemas after their parents have been anonymously extended will cause issues.
         * ALWAYS load extensions only after all integral schemas are loaded.
         */
        public string RegisterSafe(Type schemaType)
        {
            RkSchemaAttribute attribute = schemaType.GetCustomAttribute<RkSchemaAttribute>();
            Debug.Assert(attribute != null, nameof(attribute) + " != null");

            if (!SchemaInfos.ContainsKey(attribute.Identifier))
            {
                Register(attribute.Identifier, schemaType);
            }

            return attribute.Identifier;
        }

        public void Register(string schemaName, Type schemaType)
        {
            // Ensure the SchemaInfo object is initialized for the name.
            if (!SchemaInfos.ContainsKey(schemaName))
            {
                SchemaInfos[schemaName] = new();

                if (schemaType.BaseType != typeof(Schema))
                {
                    SchemaInfos[schemaName].Super = RegisterSafe(schemaType.BaseType);
                }
            }
            
            // Retrieve SchemaInfo and instantiate the builder.
            SchemaInfo info = SchemaInfos[schemaName];
            Schema schema = Activator.CreateInstance(schemaType) as Schema;
            Debug.Assert(schema != null, nameof(schema) + " != null");
            
            // Copy properties into the info object.
            CopyPropertiesIntoPropertyDict(schema.Properties, info.Properties);

            // Copy property sets into the info object.
            foreach (PropertySetDescriptor propertySet in schema.PropertySets)
            {
                string setName = NamingConvention.Convert(propertySet.Name);

                // Make sure the property dictionary is initialized.
                if (!info.PropertySets.ContainsKey(setName))
                {
                    info.PropertySets[setName] = new();
                }
                
                CopyPropertiesIntoPropertyDict(propertySet.Properties, info.PropertySets[setName]);
            }
        }

        private void CopyPropertiesIntoPropertyDict(PropertyDescriptor[] properties,
                                                    Dictionary<string, Type> propertyDict)
        {
            foreach (PropertyDescriptor property in properties)
            {
                string propertyName = NamingConvention.Convert(property.Name);

                // Throw on duplicated properties.
                // This shields schema properties from being overriden by schema extensions, or from duplicated
                // properties from builders.
                if (propertyDict.ContainsKey(propertyName))
                {
                    throw new ConstraintException("A schema may not have the same property twice at the same depth.");
                }

                propertyDict[propertyName] = property.Type;
            }
        }

        public void DeclareProperty<T>(string schemaName, string propertyPath)
            where T : new()
        {
            SchemaInfo schemaInfo = this[schemaName];

            if (!propertyPath.Contains("."))
            {
                schemaInfo.Properties[propertyPath] = typeof(T);
            }
            else
            {
                string[] pathInfo = propertyPath.Split('.', 2);
                string propertySetName = pathInfo[0];
                string propertyName = pathInfo[1];

                // Initialize the property dictionary of the set if it's missing.
                if (!schemaInfo.PropertySets.ContainsKey(propertySetName))
                {
                    schemaInfo.PropertySets[propertySetName] = new();
                }
                
                schemaInfo.PropertySets[propertySetName][propertyName] = typeof(T);
            }
        }
    }
}