using System;
using System.IO;

namespace ReaperKing.Core
{
    public struct SiteContext
    {
        public Site Site;
        public string PathPrefix;

        #region Resource Management
        public string CopyFileToLocation(string inputFile, string uri)
            => Site.CopyFileToLocation(inputFile, uri);

        public string CopyResource(string inputFile, string uri)
            => Site.CopyResource(inputFile, uri);

        public string CopyVersionedResource(string inputFile, string uri)
            => Site.CopyVersionedResource(inputFile, uri);
        #endregion
        
        #region Template Default Include Path Management
        public void AddTemplateDefaultIncludePath(string root)
            => Site.AddTemplateDefaultIncludePath(root);
        public TemplateDefaultMount TryAddTemplateDefaultIncludePath(string root)
            => new TemplateDefaultMount(Site, root);
        public TemplateDefaultMount TryAddTemplateDefaultIncludePaths(string[] roots)
            => new TemplateDefaultMount(Site, roots);
        public void RemoveTemplateDefaultIncludePath(string root)
            => Site.RemoveTemplateDefaultIncludePath(root);
        #endregion
        #region Template Scoped Include Path Management
        public void AddTemplateIncludeNamespace(string ns, string root)
            => Site.AddTemplateIncludeNamespace(ns, root);
        public TemplateNamespaceMount TryAddTemplateIncludeNamespace(string ns, string root)
            => new TemplateNamespaceMount(Site, ns, root);
        public void RemoveTemplateNamespace(string ns)
            => Site.RemoveTemplateNamespace(ns);
        public void RemoveTemplateNamespace(string ns, string root)
            => Site.RemoveTemplateNamespace(ns, root);
        #endregion
        #region Obsolete Template Include Path Management Methods
        [Obsolete("Replaced with AddTemplateDefaultIncludePath")]
        public void AddStrictTemplateDirectory(string root)
            => Site.AddTemplateDefaultIncludePath(root);
        [Obsolete("Replaced with TryAddTemplateDefaultIncludePath")]
        public TemplateDefaultMount AddOptionalTemplateDirectory(string root)
            => TryAddTemplateDefaultIncludePath(root);
        [Obsolete("Replaced with TryAddTemplateDefaultIncludePaths")]
        public TemplateDefaultMount AddOptionalTemplateDirectories(string[] roots)
            => TryAddTemplateDefaultIncludePaths(roots);
        [Obsolete("Replaced with RemoveTemplateDefaultIncludePath")]
        public void RemoveTemplateDirectory(string root)
            => RemoveTemplateDefaultIncludePath(root);
        #endregion

        #region Building Methods
        public void BuildPage(IPageGenerator generator, string uri = null)
        {
            if (PathPrefix != null)
            {
                uri = Path.Join(PathPrefix, uri);
            }
            
            Site.BuildPage(generator, uri);
        }
        public void BuildWithProvider(ISiteContentProvider provider, string uri = null)
        {
            if (PathPrefix != null)
            {
                uri = uri == null ? PathPrefix : Path.Join(PathPrefix);
            }
            
            Site.BuildWithProvider(provider, uri);
        }
        #endregion

        public bool IsConstantDefined(string id)
        {
            return Site.IsProjectConstantDefined(id);
        }
        
        [Obsolete("AcquireBaseModel is going to be removed soon. Switch to the class-based model.")]
        public BaseModel AcquireBaseModel(string siteName, string displayTitle) 
        {
            return new BaseModel(this);
        }
    }
}