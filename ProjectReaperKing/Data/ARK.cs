using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using ProjectReaperKing.Data;
using ProjectReaperKing.Data.ARK;
using ShellProgressBar;
using SiteBuilder.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

// TODO: just... uh... i don't know, refactor this in multiple ways

namespace ProjectReaperKing.Data
{
    public partial class DataManagerARK : DataManager
    {
        public static readonly DataManagerARK Instance = new DataManagerARK();
        
        public readonly Dictionary<string, MapInfo> LoadedMaps = new Dictionary<string, MapInfo>();
        public readonly Dictionary<string, ModInfo> LoadedMods = new Dictionary<string, ModInfo>();
        
        public override string GetTag() => "ark";

        public override void LoadObject(string objectName, string objectType) {
            string objectPath = objectName;
            if (objectType == "mod")
            {
                objectPath = objectName + "/mod";
            }
            
            switch (objectType)
            {
                case "mod":
                    var mod = ReadYamlFile<ModInfo>(objectPath, "mod");
                    mod.InternalId = objectName;
                    LoadedMods[objectName] = mod;
                    break;
                    
                case "map":
                    var map = ReadYamlFile<MapInfo>(objectPath, "map");
                    map.InternalId = objectName.Split('/', 2)[1];
                    LoadedMaps[objectName] = map;
                    break;
            }
        }
        
        public override void Initialize(ChildProgressBar pbar)
        {
            base.Initialize(pbar);
            var baseMessage = pbar.Message;
            
            pbar.MaxTicks += LoadedMods.Count;
            foreach (var modId in LoadedMods.Keys.ToArray())
            {
                pbar.Tick($"{baseMessage}: {modId}");
                
                var mod = LoadedMods[modId];
                mod.Revisions = _initModRevisions(pbar, modId, baseMessage).ToList();
                LoadedMods[modId] = mod;
            }
        }
        

        public IEnumerable<WorldLocation> GetNestLocations(string modId, string mapId)
        {
            var tags = new[]
            {
                RevisionTag.ModUpdate,
                RevisionTag.ModInitDataUpdate,
            };
            var revision = FindModRevisionsByTags(modId, tags).Last();
            var mapMainLevel = LoadedMaps[mapId].PersistentLevel;
            
            var liveNestsSet = revision.Item2.InitData.LiveNestSpotDefinitions;
            foreach (var nestSet in liveNestsSet)
            {
                if (nestSet.Level == mapMainLevel)
                {
                    foreach (var location in nestSet.Locations)
                    {
                        // TODO: lat long
                        yield return location;
                    }
                }
            }
        }
    }
}