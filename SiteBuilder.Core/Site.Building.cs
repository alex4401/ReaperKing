using System;
using System.IO;

namespace SiteBuilder.Core
{
    public abstract partial class Site
    {
        public async virtual void SavePage(PageGenerationResult result, string uri)
        {
            string path = Path.Join(ProjectConfig.Site.DeploymentDirectory, uri);
            string contents = await GetRazor().CompileRenderAsync(result.Template, result.Model);
            
            if (result.Uri != null)
            {
                path = Path.Join(path, result.Uri);
            }
            Directory.CreateDirectory(path);
            
            path = Path.Join(path, result.Name + ".html");

            File.WriteAllText(path, contents);
        }

        [Obsolete]
        public void BuildPage(string uri, IPageGenerator generator)
        {
            BuildPage(generator, uri);
        }
        
        public void BuildPage(IPageGenerator generator, string uri = null)
        {
            var context = new SiteContext
            {
                Site = this,
                PathPrefix = uri,
            };
            var result = generator.Generate(context);
            SavePage(result, uri);
        }

        public void BuildWithProvider(ISiteContentProvider provider, string uri = null)
        {
            var context = new SiteContext
            {
                Site = this,
                PathPrefix = uri,
            };
            provider.BuildContent(context);
        }
    }
}