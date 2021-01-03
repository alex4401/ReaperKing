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

using System.IO;
using Microsoft.Extensions.Logging;

using ReaperKing.Core;
using ReaperKing.Plugins;

using Noglin.Ark.Schemas;
using Noglin.Core;

namespace Poglin.Generation.ARK
{
    public class ArkDataContentGenerator : IDocumentProvider
    {
        private const int LookaheadSize = 256;
        
        public PackageRegistry ArkRegistry { get; init; }
        public BuildConfigurationArk Config { get; init; }
        private Site BakeRecipe { get; init; }
        private ILogger Log { get; init; }

        public ArkDataContentGenerator(Site site, ILoggerFactory logFactory)
        {
            BakeRecipe = site;
            Log = logFactory.CreateLogger<ArkDataContentGenerator>();
            Config = site.ProjectConfig.Get<BuildConfigurationArk>();

            UnifiedPackageLoader packageLoader = new(logFactory);
            ArkRegistry = new PackageRegistry(packageLoader);
            ArkRegistry.AddType<ModSpecificationSchema>();
            ArkRegistry.AddType<ProcessorConfigurationSchema>();
            
            ArkRegistry.AddType<RawSpawningGroupsData>();
            ArkRegistry.AddType<CreaturesPackage>();
            ArkRegistry.AddType<SingletonPackage>();
        }

        public void PreBuild()
        {
            ScanDirectory(BakeRecipe.ContentRoot + "/data/ark");
        }

        private void ScanDirectory(string path)
        {
            Log.LogInformation($"Scanning \"{path}\" for ARK data");
            
            foreach (string filePath in Directory.EnumerateFiles(path))
            {
                string extension = Path.GetExtension(filePath);

                // Check if the file has required fields in its head. Skip otherwise.
                bool mightBeValid = false;
                switch (extension)
                {
                    case ".yaml":
                        mightBeValid = File.ReadAllText(filePath).StartsWith("apiVersion");
                        break;
                    
                    case ".json":
                        string lookahead = File.ReadAllText(filePath).Substring(0, LookaheadSize);
                        mightBeValid = lookahead.Contains("$schema") && lookahead.Contains("format");
                        break;
                    
                    default:
                        continue;
                }

                if (!mightBeValid)
                {
                    continue;
                }

                // Load package and write it to registry
                Log.LogInformation($"Located \"{filePath}\": adding package to registry");
                ArkRegistry.LoadAnonymousPackage(filePath);
            }
            
            foreach (string directoryPath in Directory.EnumerateDirectories(path))
            {
                ScanDirectory(directoryPath);
            }
        }
        
        public void BuildContent(SiteContext ctx)
        {
            foreach (ModSpecificationSchema modSpec in ArkRegistry.Find<ModSpecificationSchema>())
            {
                Log.LogInformation($"Building data for mod: {modSpec.Meta.Name}");
                ModContentProvider provider = new ModContentProvider(this, modSpec);
                
                // Acquire a sitemap exclusion token (temporary state lock) if mod is unlisted from search engines.
                SitemapLocalExclusion? sitemapLock = null;
                if (!modSpec.Generation.AllowSitemapping)
                {
                    sitemapLock = ctx.OverrideSitemaps(false);
                }
                
                // Emit the content from the final provider.
                ctx.EmitDocumentsFrom(provider, modSpec.Meta.Tag);
                
                // Release the sitemap lock if one was acquired.
                sitemapLock?.Dispose();
            }
        }
    }
}