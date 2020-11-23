using System;

namespace ReaperKing.Core
{
    [Obsolete("Use TemplateDefaultMount for fallback mounts, or TemplateNamespaceMount for scoped mounts.")]
    public struct DisposableTemplateRoot : IDisposable
    {
        private TemplateDefaultMount _m;

        public DisposableTemplateRoot(Site site, string root)
            : this(site, new [] { root })
        { }

        public DisposableTemplateRoot(Site site, string[] roots)
        {
            _m = new TemplateDefaultMount(site, roots);
        }
        
        public void Dispose()
        {
            _m.Dispose();
        }
    }
    
    public struct TemplateDefaultMount : IDisposable
    {
        private Site _site;
        private string[] _roots;

        public TemplateDefaultMount(Site site, string root)
            : this(site, new [] { root })
        { }

        public TemplateDefaultMount(Site site, string[] roots)
        {
            _site = site;
            _roots = roots;

            foreach (var root in _roots)
            {
                _site.TryAddTemplateDefaultIncludePath(root);
            }
        }
        
        public void Dispose()
        {
            foreach (var root in _roots)
            {
                _site.RemoveTemplateDefaultIncludePath(root);
            }

            _roots = null;
            _site = null;
        }
    }
    
    public struct TemplateNamespaceMount : IDisposable
    {
        private Site _site;
        private string _ns;
        private string _root;

        public TemplateNamespaceMount(Site site, string ns, string root)
        {
            _site = site;
            _ns = ns;
            _root = root;

            _site.TryAddTemplateIncludeNamespace(ns, root);
        }

        public void Dispose()
        {
            _site.RemoveTemplateNamespace(_ns, _root);

            _ns = null;
            _root = null;
            _site = null;
        }
    }
}