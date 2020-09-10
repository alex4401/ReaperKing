using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectReaperKing.Data;
using ShellProgressBar;
using SiteBuilder.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ProjectReaperKing.Data
{
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

        public virtual void Initialize(ChildProgressBar pbar)
        {
            var baseMessage = pbar.Message;
            pbar.MaxTicks += 2;
            
            pbar.Tick($"{baseMessage}: package information");
            PackageInfo = ReadYamlFile<Package>("package");

            pbar.Tick($"{baseMessage}: objects");
            pbar.MaxTicks += PackageInfo.Objects.Count;
            foreach (KeyValuePair<string, string> kvp in PackageInfo.Objects)
            {
                pbar.Tick($"{baseMessage}: {kvp.Key}");
                LoadObject(kvp.Key, kvp.Value);
            }

            pbar.Tick(baseMessage);
        }

        public abstract void LoadObject(string objectName, string objectType);
    }
}