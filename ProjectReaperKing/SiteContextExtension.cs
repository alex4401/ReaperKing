using System.IO;
using ProjectReaperKing.Models;
using SiteBuilder.Core;

namespace ProjectReaperKing
{
    public static class SiteContextExtension
    {
        // ReSharper disable InconsistentNaming
        public static BaseModel AcquireBaseModel(this SiteContext ctx, 
                                                 string SiteName,
                                                 string DisplayTitle)
        // ReSharper restore InconsistentNaming
        {
            return new BaseModel
            {
                Ctx = ctx,
                SiteName = SiteName,
                DisplayTitle = DisplayTitle,
                Root = ctx.Site.ProjectConfig.Paths.Root,
                RootUri = (ctx.Site.ProjectConfig.Paths.Root != "/"
                           ? Path.Join(ctx.Site.ProjectConfig.Paths.Root, ctx.PathPrefix)
                           : ctx.PathPrefix),
                ResourcesDirectory = ctx.Site.ProjectConfig.Paths.Resources,
            };
        }
    }
}