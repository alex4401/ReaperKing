using System;

namespace ReaperKing.Core
{
    public struct DisposableTemplateRoot : IDisposable
    {
        private Site _site;
        private string[] _roots;

        public DisposableTemplateRoot(Site site, string root)
            : this(site, new [] { root })
        { }

        public DisposableTemplateRoot(Site site, string[] roots)
        {
            _site = site;
            _roots = roots;

            foreach (var root in _roots)
            {
                _site.AddOptionalTemplateDirectory(root);
            }
        }
        
        public void Dispose()
        {
            foreach (var root in _roots)
            {
                _site.RemoveTemplateDirectory(root);
            }

            _roots = null;
            _site = null;
        }
    }
}