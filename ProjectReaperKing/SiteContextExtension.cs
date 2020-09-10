using System.IO;
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
                SiteName = SiteName,
                DisplayTitle = DisplayTitle,
                Root = ctx.Site.ProjectConfig.Site.WebRoot,
                RootUri = Path.Join(ctx.Site.ProjectConfig.Site.WebRoot, ctx.PathPrefix),
                ResourcesDirectory = ctx.Site.ProjectConfig.Site.ResourceDirectory,
            };
        }
    }
}