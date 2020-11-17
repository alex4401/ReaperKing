using System;
using System.IO;
using Microsoft.Extensions.Logging;
using NUglify;

namespace ReaperKing.Core
{
    public abstract partial class Site
    {
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