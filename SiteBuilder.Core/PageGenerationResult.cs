using System;

namespace SiteBuilder.Core
{
    public struct PageGenerationResult
    {
        public string Uri;
        public string Name;
        
        public string Template;
        public object Model;
    }
}