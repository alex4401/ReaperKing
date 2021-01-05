/*!
 * This file is a part of the Poglin project, whose repository may be found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General
 * Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Affero General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;
using ReaperKing.Core.Configuration;
using ReaperKing.CommonTemplates.Extensions;
using ReaperKing.Anhydrate.Extensions;
using ReaperKing.Plugins;

using Poglin.Generation.ARK;
using Poglin.Generation.Tools;

namespace Poglin.StaticConfiguration
{
    [SiteRecipe]
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class RkBuildRecipe : Site
    {
        private ArkDataContentGenerator _arkContentGenerator;
        
        public RkBuildRecipe(ProjectConfigurationManager project,
                             ILoggerFactory loggerFactory)
            : base(typeof(RkBuildRecipe), project, loggerFactory)
        {
            AddModule(new RkFandomImageVirtualFsModule(this));
            AddModule(new RkSitemapExclusionModule(this));
            AddModule(new RkUglifyModule(this));
            AddModule(new RkDocumentCollectionModule(this));
            AddModule(new RkImageOptimizationModule(this));
        }

        public override void PreBuild()
        {
            base.PreBuild();
            
            _arkContentGenerator = new ArkDataContentGenerator(this, LogFactory);
            _arkContentGenerator.PreBuild();
        }

        public override void Build()
        {
            this.EnableCommonTemplates();
            this.EnableAnhydrateTemplates();

            Log.LogInformation("Building ARK tools");
            using (this.OverrideSitemaps(false))
            {
                EmitDocumentsFrom<ToolsContentProvider>(new(), "/ark/tools");
                EmitDocumentsFrom<ToolsRedirectsProvider>(new(), "/wiki/tools");
            }

            Log.LogInformation("Building ARK mod content");
            EmitDocumentsFrom(_arkContentGenerator, "/ark");
        }

        public override void PostBuild()
        {
            base.PostBuild();

            Log.LogInformation("Creating a sitemap");
            {
                var module = GetModuleInstance<RkDocumentCollectionModule>();
                EmitDocument<SitemapGenerator>(new(module));
            }
        }
    }
}