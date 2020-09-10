using SiteBuilder.Core;

namespace ProjectReaperKing.Models
{
    public struct BaseModel
    {
        public SiteContext Ctx;
        public string SiteName;
        public string DisplayTitle;
        public string Root;
        public string RootUri;
        public string ResourcesDirectory;
        
        public string CopyVersionedResource(string inputFile, string uri)
            => Ctx.CopyVersionedResource(inputFile, uri);
    }
}