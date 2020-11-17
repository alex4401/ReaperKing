using System.IO;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;
using ReaperKing.CommonTemplates.Extensions;
using ReaperKing.Generation.ARK;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.Tools;

namespace ReaperKing.StaticConfig
{
    [Site]
    // ReSharper disable once UnusedType.Global
    public class RkSiteBuildRecipe : Site
    {
        public override void PreBuild()
        {
            base.PreBuild();
            
            var dataManager = DataManagerARK.Instance;
            Log.LogInformation("Discovering and loading ARK data");
            dataManager.Initialize(Log);
        }

        public override void Build()
        {
            this.EnableCommonTemplates();
            
            var dataManager = DataManagerARK.Instance;
            
            Log.LogInformation("Building ARK tools");
            {
                BuildWithProvider(new ToolsContentProvider(), "/ark/tools");
                BuildWithProvider(new ToolsRedirectsProvider(), "/wiki/tools");
            }

            Log.LogInformation("Building ARK mod content");
            {
                foreach (string modTag in dataManager.LoadedMods.Keys)
                {
                    var uri = Path.Join("/ark", modTag);
                    var generator = new ModContentProvider(modTag);
                    BuildWithProvider(generator, uri);
                }
            }
        }
    }
}