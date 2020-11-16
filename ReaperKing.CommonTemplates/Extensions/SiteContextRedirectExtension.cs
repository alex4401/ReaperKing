using ReaperKing.Core;

namespace ReaperKing.CommonTemplates.Extensions
{
    public static class SiteContextRedirectExtension
    {
        public static void RedirectPage(this SiteContext ctx, string source, string target)
        {
            ctx.BuildPage(new RedirectGenerator
            {
                Name = source,
                TargetUrl = target,
            });
        }
        
        public static void RedirectPage(this SiteContext ctx, string uri, string source, string target)
        {
            ctx.BuildPage(new RedirectGenerator
            {
                Uri = uri,
                Name = source,
                TargetUrl = target,
            });
        }
    }
}