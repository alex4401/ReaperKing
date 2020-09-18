using System;
using System.IO;
using NUglify;

namespace SiteBuilder.Core
{
    public abstract partial class Site
    {
        public async void SavePage(PageGenerationResult result, string uri)
        {
            string path = Path.Join(DeploymentPath, uri);
            string contents = await GetRazor().CompileRenderAsync(result.Template, result.Model);

            if (ProjectConfig.Build.MinifyHtml)
            {
                var uglifyResult = Uglify.Html(contents);
                if (uglifyResult.HasErrors)
                {
                    // TODO: print the errors
                }
                else
                {
                    contents = uglifyResult.Code;
                }
            }

            if (result.Uri != null)
            {
                path = Path.Join(path, result.Uri);
            }
            Directory.CreateDirectory(path);
            
            path = Path.Join(path, result.Name + ".html");

            File.WriteAllText(path, contents);
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