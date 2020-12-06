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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Logging;

using ReaperKing.Anhydrate;
using ReaperKing.Core;
using ReaperKing.CommonTemplates.Extensions;
using ReaperKing.Anhydrate.Extensions;
using ReaperKing.Generation.ARK;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.Tools;
using ReaperKing.Plugins;

namespace ReaperKing.StaticConfig
{
    [SiteRecipe]
    /*[RkConfigurable(new[]
    {
        typeof(RkFandomImageVirtualFsModule),
        typeof(RkSitemapExclusionModule),
        typeof(RkUglifyModule),
        typeof(RkDocumentCollectionModule),
        typeof(RkImageOptimizationModule),
    })]*/
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class RkBuildRecipe : Site
    {
        public RkBuildRecipe(ProjectConfigurationManager project,
                             ILoggerFactory loggerFactory)
            : base(typeof(RkBuildRecipe), project, loggerFactory)
        {
            AddModule(new RkFandomImageVirtualFsModule(this));
            AddModule(new RkSitemapExclusionModule(this));
            AddModule(new RkUglifyModule(this));
            AddModule(new RkDocumentCollectionModule(this));
            AddModule(new RkImageOptimizationModule(this));
            
            ProjectConfig.AddType<AnhydrateConfiguration>();
        }

        public override void PreBuild()
        {
            base.PreBuild();
            
            var dataManager = DataManagerARK.Instance;
            Log.LogInformation("Discovering and loading ARK data");
            dataManager.Initialize(LogFactory.CreateLogger<DataManagerARK>());
        }

        public override void Build()
        {
            this.EnableCommonTemplates();
            this.EnableAnhydrateTemplates();

            Log.LogInformation("Building ARK tools");
            using (this.OverrideSitemaps(false))
            {
                EmitDocumentsFrom(new ToolsContentProvider(), "/ark/tools");
                EmitDocumentsFrom(new ToolsRedirectsProvider(), "/wiki/tools");
            }

            Log.LogInformation("Building ARK mod content");
            foreach (string modTag in DataManagerARK.Instance.LoadedMods.Keys)
            {
                var uri = Path.Join("/ark", modTag);
                var generator = new ModContentProvider(modTag);
                EmitDocumentsFrom(generator, uri);
            }
        }

        public override void PostBuild()
        {
            base.PostBuild();

            Log.LogInformation("Creating a sitemap");
            {
                var module = GetModuleInstance<RkDocumentCollectionModule>();
                EmitDocument(new SitemapGenerator(module));
            }
        }
    }
}