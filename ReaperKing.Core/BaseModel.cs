namespace ReaperKing.Core
{
    public record BaseModel
    {
        public SiteContext Ctx { get; }
        public string Root { get; }
        public string RootUri { get; }
        public string ResourcesDirectory { get; }
        
        public BaseModel(SiteContext ctx)
            => (Ctx,
                Root,
                RootUri,
                ResourcesDirectory)
                = (ctx,
                    ctx.Site.ProjectConfig.Paths.Root,
                    ctx.GetRootUri(),
                    ctx.Site.ProjectConfig.Paths.Resources);
        
        public string CopyVersionedResource(string inputFile, string uri)
            => Ctx.CopyVersionedResource(inputFile, uri);
    }
}