using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Noglin.Ark.Schemas;
using Noglin.Core;
using ReaperKing.Core;
using ReaperKing.Plugins;

namespace ReaperKing.Generation.ARK
{
    public class ArkDataContentGenerator : ISiteContentProvider
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

                // Check if the file has required fields in its head.
                // Skip otherwise.
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
                
                // Acquire a sitemap exclusion token (temporary state
                // lock) if mod is unlisted from search engines.
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