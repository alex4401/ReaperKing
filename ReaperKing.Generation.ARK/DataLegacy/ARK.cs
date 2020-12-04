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
using System.Linq;
using Microsoft.Extensions.Logging;

// TODO: just... uh... i don't know, refactor this in multiple ways

namespace ReaperKing.Generation.ARK.Data
{
    [Obsolete("DataManager will soon be replaced with Noglin.")]
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