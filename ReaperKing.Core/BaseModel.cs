using System;
using System.IO;

namespace ReaperKing.Core
{
    public class BaseModel
    {
        public BaseModel(SiteContext ctx)
        {
            Ctx = ctx;
            Root = ctx.Site.ProjectConfig.Paths.Root;
            RootUri = (ctx.Site.ProjectConfig.Paths.Root != "/"
                ? Path.Join(ctx.Site.ProjectConfig.Paths.Root, ctx.PathPrefix)
                : ctx.PathPrefix);
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