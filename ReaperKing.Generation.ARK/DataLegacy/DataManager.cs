/*!
 * This file is a part of the open-sourced engine modules for
 * https://alex4401.github.io, and those modules' repository may be found
 * at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;

namespace ReaperKing.Generation.ARK.Data
{
    [Obsolete("DataManager will soon be replaced with Noglin.")]
    public abstract class DataManager
    {
        protected Package PackageInfo;

        protected T ReadYamlFile<T>(string filePath)
        {
            string fullPath = Path.Join(GetDataDirectoryPath(), filePath) + ".yaml";
            return ParsingUtils.ReadYamlFile<T>(fullPath);
        }

        protected T ReadYamlFile<T>(string filePath, string field)
        {
            string fullPath = Path.Join(GetDataDirectoryPath(), filePath) + ".yaml";
            return ParsingUtils.ReadYamlFile<T>(fullPath, field);
        }

        protected T ReadJsonFile<T>(string filePath)
        {
            string fullPath = Path.Join(GetDataDirectoryPath(), filePath) + ".json";
            return ParsingUtils.ReadJsonFile<T>(fullPath);
        }
        
        public abstract string GetTag();
        public virtual string GetDataDirectoryPath()
        {
            return Path.Join(Environment.CurrentDirectory, "data", GetTag());
        }

        public virtual void Initialize(ILogger log)
        {
            log.LogInformation("Package information is being loaded");
            PackageInfo = ReadYamlFile<Package>("package");

            log.LogInformation($"{PackageInfo.Objects.Count} objects referenced by the package");
            foreach (KeyValuePair<string, string> kvp in PackageInfo.Objects)
            {
                log.LogInformation($"Object \"{kvp.Key}\" is being loaded");
                LoadObject(kvp.Key, kvp.Value);
            }
        }

        public abstract void LoadObject(string objectName, string objectType);
    }
}