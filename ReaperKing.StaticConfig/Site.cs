using System.IO;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;
using ReaperKing.CommonTemplates.Extensions;
using ReaperKing.Anhydrate.Extensions;
using ReaperKing.Generation.ARK;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.Tools;
using ReaperKing.Plugins;

namespace ReaperKing.StaticConfig
{
    [Site]
    // ReSharper disable once UnusedType.Global
    public class RkSiteBuildRecipe : Site
    {
        public RkSiteBuildRecipe(Project project, ILogger logger)
            : base(project, logger)
        {
            // HACK: Compatibility.
            FandomUtils.SiteInstance = this;
            
            AddModule(new RkSitemapExclusionModule(this));
            AddModule(new RkUglifyModule(this));
            AddModule(new RkDocumentCollectionModule(this));

            if (IsProjectConstantDefined("WIP_IMAGE_OPTIMIZATION"))
            {
                AddModule(new RkImageOptimizationModule(this));
            }
        }

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
            this.EnableAnhydrateTemplates();
            
            Log.LogInformation("Building ARK tools");
            using (this.OverrideSitemaps(false)) {
                BuildWithProvider(new ToolsContentProvider(), "/ark/tools");
                BuildWithProvider(new ToolsRedirectsProvider(), "/wiki/tools");
            }

            Log.LogInformation("Building ARK mod content");
            {
                foreach (string modTag in DataManagerARK.Instance.LoadedMods.Keys)
                {
                    var uri = Path.Join("/ark", modTag);
                    var generator = new ModContentProvider(modTag);
                    BuildWithProvider(generator, uri);
                }
            }
        }

        public override void PostBuild()
        {
            base.PostBuild();

            Log.LogInformation("Creating a sitemap");
            {
                var module = GetModuleInstance<RkDocumentCollectionModule>();
                BuildPage(new SitemapGenerator(module));
            }
        }
    }
}