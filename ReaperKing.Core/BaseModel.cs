namespace ReaperKing.Core
{
    public class BaseModel
    {
        public BaseModel(SiteContext ctx)
        {
            Ctx = ctx;
            Root = ctx.Site.ProjectConfig.Paths.Root;
            RootUri = ctx.GetRootUri();
            ResourcesDirectory = ctx.Site.ProjectConfig.Paths.Resources;
        }
        
        public SiteContext Ctx { get; }
        public string Root { get; }
        public string RootUri { get; }
        public string ResourcesDirectory { get; }
        
        public string CopyVersionedResource(string inputFile, string uri)
            => Ctx.CopyVersionedResource(inputFile, uri);
    }
}