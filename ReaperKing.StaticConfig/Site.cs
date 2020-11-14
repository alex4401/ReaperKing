using System;
using System.Collections.Generic;
using System.IO;
using ShellProgressBar;
using ReaperKing.Core;
using ReaperKing.StaticConfig.ContentGeneration.ARK;
using ReaperKing.StaticConfig.ContentGeneration.WikiTools;
using ReaperKing.StaticConfig.Data;

namespace ReaperKing.StaticConfig
{
    [Site]
    public class ProjectReaperKingSite : Site
    {
        public override void Build(ProgressBar pbar)
        {
            base.Build(pbar);
            
            pbar.MaxTicks += 1;
            var dataManager = DataManagerARK.Instance;
            
            using (var pbar2 = pbar.Spawn(1, "Discovering and loading data", BarOptions))
            {
                dataManager.Initialize(pbar2);
            }
            pbar.Tick();
            
            using (var pbar2 = pbar.Spawn(1, "Building content for wiki tools", BarOptions))
            {
                var generator = new WikiToolsContentProvider();
                BuildWithProvider(generator, "/wiki/tools");
                pbar2.Tick();
            }
            
            using (var pbar2 = pbar.Spawn(dataManager.LoadedMods.Count, "Building content for mods", BarOptions))
            {
                foreach (string modTag in dataManager.LoadedMods.Keys)
                {
                    pbar2.Tick($"Building content for mod: {modTag}");
                    
                    var uri = Path.Join("/ark", modTag);
                    var generator = new ModContentProvider(modTag);
                    BuildWithProvider(generator, uri);
                }
            }
        }
    }
}