using System;

namespace ReaperKing.Core
{
    public struct PageGenerationResult
    {
        public string Extension;
        public string Uri;
        public string Name;
        public bool SkipInSitemap;
        
        public string Text;
        public string Template;
        public object Model;
    }
}