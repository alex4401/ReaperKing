using System;
using System.IO;

namespace SiteBuilder.Core
{
    public abstract partial class Site
    {
        public async virtual void BuildPage(string uri, IPageGenerator generator)
        {
            var result = generator.Generate(this, uri);
            string path = Path.Join(ProjectConfig.Site.DeploymentDirectory, uri);
            
            if (result.Uri != null)
            {
                path = Path.Join(path, result.Uri);
            }
            Directory.CreateDirectory(path);
            
            path = Path.Join(path, result.Name + ".html");
            string contents = await GetRazor().CompileRenderAsync(result.Template, result.Model);

            File.WriteAllText(path, contents);
        }

        [Obsolete]
        public virtual void BuildIndex(string uri, IPageIndex index)
        {
            string uri2 = Path.Join(uri, index.GetPath());
                
            var pages = index.GetAll(this);
            foreach (var page in pages)
            {
                BuildPage(uri2, page);
            }
        }

        [Obsolete]
        public void BuildIndices(string uri, IPageIndex[] indices)
        {
            foreach (var index in indices)
            {
                BuildIndex(uri, index);
            }
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