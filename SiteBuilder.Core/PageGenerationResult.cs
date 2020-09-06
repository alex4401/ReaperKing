using System;

namespace SiteBuilder.Core
{
    public struct PageGenerationResult
    {
        public string Uri;
        public string Name;

        [Obsolete]
        public bool WantsAsyncRender;
        [Obsolete]
        public string Contents;
        
        public string Template;
        public object Model;
    }
}