using System.ComponentModel;
using System.IO;
using System.Linq;
using RazorLight;

namespace SiteBuilder.Core
{
    public struct SiteContext
    {
        public Site Site;
        public string PathPrefix;

        public string CopyFileToLocation(string inputFile, string uri)
            => Site.CopyFileToLocation(inputFile, uri);

        public string CopyResource(string inputFile, string uri)
            => Site.CopyResource(inputFile, uri);

        public string CopyVersionedResource(string inputFile, string uri)
            => Site.CopyVersionedResource(inputFile, uri);

        public void BuildPage(IPageGenerator generator, string uri = null)
        {
            if (PathPrefix != null)
            {
                uri = Path.Join(PathPrefix, uri);
            }
            
            Site.BuildPage(generator, uri);
        }

        public void AddStrictTemplateDirectory(string root)
            => Site.AddTemplateDirectory(root);
        public DisposableTemplateRoot AddOptionalTemplateDirectory(string root)
            => new DisposableTemplateRoot(Site, root);
        public DisposableTemplateRoot AddOptionalTemplateDirectories(string[] roots)
            => new DisposableTemplateRoot(Site, roots);
        public void RemoveTemplateDirectory(string root)
            => Site.RemoveTemplateDirectory(root);

        public void BuildWithProvider(ISiteContentProvider provider, string uri = null)
        {
            if (PathPrefix != null)
            {
                uri = uri == null ? PathPrefix : Path.Join(PathPrefix);
            }
            
            Site.BuildWithProvider(provider, uri);
        }

        public bool IsConstantDefined(string id)
        {
            return Site.ProjectConfig.Build.Define != null && Site.ProjectConfig.Build.Define.Contains(id);
        }
    }
}