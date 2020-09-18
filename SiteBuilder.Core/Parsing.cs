using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text.Json;
using SharpYaml;
using SharpYaml.Serialization;

namespace SiteBuilder.Core
{
    public static class ParsingUtils
    {
        public static readonly Serializer YamlReader = new Serializer(
            new SerializerSettings {
                NamingConvention = new CamelCaseNamingConvention(),
            });
        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        public static T ReadYamlFile<T>(string filePath)
        {
            T package = YamlReader.Deserialize<T>(File.ReadAllText(filePath));
            return package;
        }
        
        public static T ReadYamlFile<T>(string filePath, T output)
        {
            return YamlReader.DeserializeInto<T>(File.ReadAllText(filePath), output);
        }

        public static T ReadYamlFile<T>(string filePath, string field)
        {
            var package = YamlReader.Deserialize<Dictionary<string, T>>(File.ReadAllText(filePath));
            return package[field];
        }
        
        public static T ReadJsonFile<T>(string filePath)
        {
            T package = JsonSerializer.Deserialize<T>(File.ReadAllText(filePath), JsonOptions);
            return package;
        }
    }
}