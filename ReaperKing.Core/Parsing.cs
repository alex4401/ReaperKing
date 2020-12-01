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

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SharpYaml.Serialization;

namespace ReaperKing.Core
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