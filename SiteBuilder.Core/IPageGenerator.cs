using System;
using System.Collections.Generic;

namespace SiteBuilder.Core
{

    [Obsolete]
    public interface IPageIndex
    {
        public IEnumerable<IPageGenerator> GetAll(Site site);
        public string GetPath();
    }
    
    public interface IPageGenerator
    {
        public PageGenerationResult Generate(Site site, string parentUri);
    }
}