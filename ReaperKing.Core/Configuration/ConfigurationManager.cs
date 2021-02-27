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
using System.IO;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using SharpYaml;
using SharpYaml.Events;
using SharpYaml.Serialization;

namespace ReaperKing.Core.Configuration
{
    public class ProjectConfigurationManager
    {
        private static readonly CamelCaseNamingConvention NamingConvention = new();
        
        public SchemaCollectionManager SchemaManager { get; }
        private ILogger Log { get; }
        private readonly Dictionary<Type, object> _objectPool = new();

        public ProjectConfigurationManager(ILoggerFactory logFactory)
            : this(logFactory, new())
        { }

        public ProjectConfigurationManager(ILoggerFactory logFactory, SchemaCollectionManager schemaManager)
        {
            Log = logFactory.CreateLogger<ProjectConfigurationManager>();
            SchemaManager = schemaManager;
            UpdateTypesFromSchemaManager("ReaperKing/Minimal/v*");
        }
        
        public void InitType(Type type)
        {
            if (_objectPool.ContainsKey(type))
            {
                return;
            }
            
            // Initialize the object in cache to serve defaults.
            _objectPool[type] = Activator.CreateInstance(type);
        }

        public void InitType<T>()
            where T : new()
            => InitType(typeof(T));

        public void Override<T>(T obj)
            => _objectPool[typeof(T)] = obj;

        public object this[Type type]
            => _objectPool[type];
        
        /**
         * Retrieves a configured object of type T.
         */
        public T Get<T>()
            => (T) this[typeof(T)];

        public void UpdateTypesFromSchemaManager(string schemaName)
        {
            SchemaCollectionManager.SchemaInfo info = SchemaManager[schemaName];

            if (!String.IsNullOrEmpty(info.Super))
            {
                UpdateTypesFromSchemaManager(info.Super);
            }
            
            foreach ((string _, Type type) in info.Properties)
            {
                InitType(type);
            }

            foreach ((string _, Dictionary<string, Type> properties) in info.PropertySets)
            {
                foreach ((string _, Type type) in properties)
                {
                    InitType(type);
                }
            }
        }

        public void InjectProperty<T>(string propertyPath)
            where T : new()
            => SchemaManager.DeclareProperty<T>("*", propertyPath);

        public void LoadFile(string path)
        {
            using StringReader reader = new(File.ReadAllText(path));
            
            Parser parser = new Parser(reader);
            EventReader eventStream = new EventReader(parser);

            eventStream.Expect<StreamStart>();
            eventStream.Expect<DocumentStart>();
            eventStream.Expect<MappingStart>();

            string schemaName = CheckFileSchemaType(eventStream);
            Log.LogDebug($"Schema \"{schemaName}\" has been specified for \"{path}\"");
            if (schemaName == null)
            {
                throw new SerializationException($"Failed to determine the property set for \"{path}\".");
            }
            UpdateTypesFromSchemaManager(schemaName);

            while (!eventStream.Accept<MappingEnd>())
            {
                if (eventStream.Accept<MappingStart>())
                {
                    // Depth will be increased as a mapping has been encountered. This is most likely a left-over from
                    // a previous node.
                    eventStream.Skip();
                    continue;
                }
                    
                Scalar fieldScalar = eventStream.Expect<Scalar>();
                string fieldName = NamingConvention.Convert(fieldScalar.Value);

                // Attempt to load the field as a property.
                Type propertyType = FindPropertyByName(schemaName, fieldName);
                if (propertyType != null)
                {
                    // The field is a property.
                    Log.LogDebug($"Found top-level property: {fieldName}");
                    
                    InitType(propertyType);
                    _objectPool[propertyType] = ParsingUtils.YamlReader.Deserialize(eventStream, propertyType,
                                                                                    _objectPool[propertyType]);
                    
                    // Notify the property that its values may have changed.
                    if (_objectPool[propertyType] is IRkConfigNotifiedWhenUpdated propertyNotifiable)
                    {
                        propertyNotifiable.OnUpdated();
                    }
                    
                    continue;
                }
                
                // Attempt to load the field as a property set.
                if (IsPropertySetByName(schemaName, fieldName))
                {
                    Log.LogDebug($"Found top-level property set: {fieldName}");
                    
                    eventStream.Expect<MappingStart>();
                    while (!eventStream.Accept<MappingEnd>())
                    {
                        Scalar propertyScalar = eventStream.Expect<Scalar>();
                        string propertyName = NamingConvention.Convert(propertyScalar.Value);

                        propertyType = FindPropertyByNameInSet(schemaName, propertyName, fieldName);
                        if (propertyType == null)
                        {
                            throw new SerializationException($"Encountered unknown property \"{propertyName}\" "
                                                             + $"in property set \"{fieldName}\".");
                        }

                        Log.LogDebug($"Found property \"{propertyName}\" in set \"{fieldName}\"");
                        InitType(propertyType);
                        _objectPool[propertyType] = ParsingUtils.YamlReader.Deserialize(eventStream, propertyType,
                                                                                        _objectPool[propertyType]);

                        // Notify the property that its values may have changed.
                        if (_objectPool[propertyType] is IRkConfigNotifiedWhenUpdated propertyNotifiable)
                        {
                            propertyNotifiable.OnUpdated();
                        }
                    }

                    eventStream.Expect<MappingEnd>();
                    continue;
                }
                
                Log.LogWarning($"Found an unknown field in the project configuration: {fieldName}");
            }
        }
        
        private string CheckFileSchemaType(EventReader eventStream)
        {
            Scalar key = eventStream.Expect<Scalar>();
            Scalar value = eventStream.Expect<Scalar>();

            if (key.Value != "apiVersion")
            {
                Log.LogCritical("Could not find a schema version in a project configuration file to be loaded.");
                LogKnownSchemas(LogLevel.Critical);
                return null;
            }

            if (!SchemaManager.IsValid(value.Value))
            {
                Log.LogCritical("Tried to load a configuration file, but its schema \"" + value.Value + "\" "
                                + "is not known.");
                LogKnownSchemas(LogLevel.Critical);
                return null;
            }

            return value.Value;
        }

        private void LogKnownSchemas(LogLevel logLevel)
        {
            Log.Log(logLevel, "Known schemas:");

            foreach (string name in SchemaManager.Names)
            {
                if (name == "*")
                {
                    continue;
                }
                
                Log.Log(logLevel, "- " + name);
            }
        }

        private Type FindPropertyByName(string schemaName, string name)
        {
            // Return null if look-up was requested to be in the global schema, but such schema does not exist.
            if (schemaName == "*" && !SchemaManager.IsValid("*"))
            {
                return null;
            }
            
            SchemaCollectionManager.SchemaInfo schemaInfo = SchemaManager[schemaName];
            // Perform the look-up.
            if (schemaInfo.Properties.ContainsKey(name))
            {
                return schemaInfo.Properties[name];
            }

            // Retrieve upper level's name (or global schema instead).
            string super = schemaInfo.Super;
            if (String.IsNullOrEmpty(schemaInfo.Super))
            {
                super = "*";
            }

            // Proceed to go up the tree if upper level is not the same as current.
            if (super != schemaName)
            {
                return FindPropertyByName(super, name);
            }

            return null;
        }

        private bool IsPropertySetByName(string schemaName, string name)
        {
            // Return false if look-up was requested to be in the global schema, but such schema does not exist.
            if (schemaName == "*" && !SchemaManager.IsValid("*"))
            {
                return false;
            }
            
            SchemaCollectionManager.SchemaInfo schemaInfo = SchemaManager[schemaName];
            // Perform the look-up.
            if (schemaInfo.PropertySets.ContainsKey(name))
            {
                return true;
            }
            
            // Retrieve upper level's name (or global schema instead).
            string super = schemaInfo.Super;
            if (String.IsNullOrEmpty(schemaInfo.Super))
            {
                super = "*";
            }

            // Proceed to go up the tree if upper level is not the same as current.
            if (super != schemaName)
            {
                return IsPropertySetByName(super, name);
            }

            return false;
        }

        private Type FindPropertyByNameInSet(string schemaName, string name, string set)
        {
            // Return null if look-up was requested to be in the global schema, but such schema does not exist.
            if (schemaName == "*" && !SchemaManager.IsValid("*"))
            {
                return null;
            }
            
            SchemaCollectionManager.SchemaInfo schemaInfo = SchemaManager[schemaName];
            // Perform the look-up.
            if (schemaInfo.PropertySets.ContainsKey(set))
            {
                Dictionary<string, Type> propertySet = schemaInfo.PropertySets[set];
                if (propertySet.ContainsKey(name))
                {
                    return propertySet[name];
                }
            }

            // Retrieve upper level's name (or global schema instead).
            string super = schemaInfo.Super;
            if (String.IsNullOrEmpty(schemaInfo.Super))
            {
                super = "*";
            }

            // Proceed to go up the tree if upper level is not the same as current.
            if (super != schemaName)
            {
                return FindPropertyByNameInSet(super, name, set);
            }

            return null;
        }
    }
}