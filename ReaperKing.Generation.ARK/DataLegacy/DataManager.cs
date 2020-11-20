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