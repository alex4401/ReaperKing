/*!
 * This file is a part of Reaper King, and the project's repository may be
 * found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See
 * the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using SharpYaml;
using SharpYaml.Events;
using SharpYaml.Serialization;

namespace ReaperKing.Core
{
    #region Structures
    [RkProjectProperty("rk", "web")]
    public record WebConfiguration
    {
        public string Root { get; init; } = "/";
        public string Resources { get; init; } = "/assets";
        public string ExternalAddress { get; init; } = "http://example.localhost";
    }
    
    [RkProjectProperty("rk", "resources")]
    public record ResourcesConfiguration
    {
        public List<string> CopyNonVersioned { get; init; } = new();
    }
    
    [RkProjectProperty("rk", "_runtimeImmutable")]
    public record ImmutableRuntimeConfiguration
    {
        public string ContentRoot { get; init; }
        public string AssemblyRoot { get; init; }
        public string DeploymentPath { get; init; }
    }
    #endregion

    
    public class ProjectConfigurationManager
    {
        private struct PropertyDescriptor
        {
            public string PropertyNamespace { get; init; }
            public string PropertyName { get; init; }
            public Type PropertyType { get; init; }
        }
        
        private static readonly CamelCaseNamingConvention NamingConvention = new();
        
        private readonly Dictionary<Type, object> _cache = new();
        private readonly List<PropertyDescriptor> _types = new();

        /*
         * Adds a configuration record as a valid configuration element.
         *
         * Generic argument T is expected to have an RkProjectProperty
         * attribute.
         */
        public void AddType(Type type)
        {
            // Retrieve the project property attribute.
            var attribute = type.GetCustomAttribute<RkProjectPropertyAttribute>();

            if (attribute == null)
            {
                throw new ArgumentException($"{type.FullName} does not have an RkProjectProperty " +
                                            "attribute and thus cannot be exposed to project configurations.");
            }
            
            // Push a descriptor of the property T.
            _types.Add(new PropertyDescriptor
            {
                PropertyNamespace = NamingConvention.Convert(attribute.Namespace),
                PropertyName = NamingConvention.Convert(attribute.Name),
                PropertyType = type,
            });

            // Initialize the object in cache to serve defaults.
            _cache[type] = Activator.CreateInstance(type);
        }

        /*
         * Adds a configuration record as a valid configuration element.
         *
         * Generic argument T is expected to have an RkProjectProperty
         * attribute.
         */
        public void AddType<T>()
        {
            AddType(typeof(T));
        }

        /*
         * Adds a configuration record as a valid configuration element.
         *
         * Generic argument T is expected to have an RkProjectProperty
         * attribute. The parameter is expected to be valid.
         */
        public void Override<T>(T obj)
        {
            Type type = typeof(T);
            _cache[type] = obj;
        }

        public void ScanType(Type type)
        {
            foreach (RkConfigurableAttribute attribute in type.GetCustomAttributes<RkConfigurableAttribute>())
            {
                foreach (Type propertyType in attribute.Properties)
                {
                    if (propertyType.GetCustomAttributes<RkProjectPropertyAttribute>().Any())
                    {
                        AddType(propertyType);
                    }
                    else
                    {
                        ScanType(propertyType);
                    }
                }
            }
        }

        public void LoadFile(string path)
        {
            using (StringReader reader = new(File.ReadAllText(path)))
            {
                Parser parser = new Parser(reader);
                EventReader eventStream = new EventReader(parser);

                eventStream.Expect<StreamStart>();
                eventStream.Expect<DocumentStart>();
                eventStream.Expect<MappingStart>();
                
                while (!eventStream.Accept<MappingEnd>())
                {
                    if (eventStream.Accept<MappingStart>())
                    {
                        // Depth will be increased as a mapping has been encountered.
                        // This is most likely a left-over from previous node.
                        eventStream.Skip();
                        continue;
                    }
                    
                    var namespaceNameEvent = eventStream.Expect<Scalar>();
                    string namespaceName = NamingConvention.Convert(namespaceNameEvent.Value);
                    PropertyDescriptor[] properties = _types.Where(
                        info => namespaceName == info.PropertyNamespace
                    ).ToArray();

                    if (properties.Length == 0)
                    {
                        if (eventStream.Accept<MappingStart>())
                        {
                            throw new SerializationException($"Encountered an unknown field set \"{namespaceName}\".");
                        }

                        continue;
                    }

                    eventStream.Expect<MappingStart>();
                    while (!eventStream.Accept<MappingEnd>())
                    {
                        var propertyNameEvent = eventStream.Expect<Scalar>();

                        string propertyName = NamingConvention.Convert(propertyNameEvent.Value);
                        Type propertyType = properties.FirstOrDefault(
                            info => propertyName == info.PropertyName
                        ).PropertyType;
                        if (propertyType == null)
                        {
                            throw new SerializationException($"Encountered an unknown field \"{propertyName}\" in set \"{namespaceName}\".");
                        }

                        _cache[propertyType] = ParsingUtils.YamlReader.Deserialize(eventStream, propertyType, _cache[propertyType]);
                    }

                    eventStream.Expect<MappingEnd>();
                }
            }
        }

        /**
         * Retrieves a configured object of type T.
         */
        public T Get<T>()
        {
            var type = typeof(T);
            return (T) _cache[type];
        }
    }
    
}