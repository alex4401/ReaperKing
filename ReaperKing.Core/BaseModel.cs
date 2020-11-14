using System;

namespace ReaperKing.Core
{
    public struct BaseModel
    {
        public SiteContext Ctx;
        [Obsolete] public string SiteName;
        [Obsolete]  public string DisplayTitle;
        public string Root;
        public string RootUri;
        public string ResourcesDirectory;
        
        public string CopyVersionedResource(string inputFile, string uri)
            => Ctx.CopyVersionedResource(inputFile, uri);
    }
}