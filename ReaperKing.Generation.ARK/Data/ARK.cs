using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

// TODO: just... uh... i don't know, refactor this in multiple ways

namespace ReaperKing.Generation.ARK.Data
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
        
        public override void Initialize(ILogger log)
        {
            base.Initialize(log);
            
            foreach (var modId in LoadedMods.Keys.ToArray())
            {
                var mod = LoadedMods[modId];
                mod.Revisions = _initModRevisions(log, modId).ToList();
                LoadedMods[modId] = mod;
            }
        }
        
    }
}