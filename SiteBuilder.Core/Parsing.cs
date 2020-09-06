using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SiteBuilder.Core
{
    [Obsolete]
    public static class YamlUtils
    {
        [Obsolete]
        public static T ReadYamlFile<T>(string filePath)
        {
            return ParsingUtils.ReadYamlFile<T>(filePath);
        }

        [Obsolete]
        public static T ReadYamlFile<T>(string filePath, string field)
        {
            return ParsingUtils.ReadYamlFile<T>(filePath, field);
        }
    }
    
    public static class ParsingUtils
    {
        public static readonly IDeserializer YamlReader = 
            new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
        public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        public static T ReadYamlFile<T>(string filePath)
        {
            T package = YamlReader.Deserialize<T>(File.ReadAllText(filePath));
            return package;
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