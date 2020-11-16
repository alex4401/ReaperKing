using ReaperKing.Core;
using ReaperKing.CommonTemplates.Extensions;

namespace ReaperKing.Generation.Tools
{
    public class ToolsRedirectsProvider : ISiteContentProvider
    {
        public void BuildContent(SiteContext ctx)
        {
            string root = ctx.Site.ProjectConfig.Paths.Root;
            
            ctx.RedirectPage("dv-json", $"{root}/ark/tools/dv-json.html");
            ctx.RedirectPage("color-table", $"{root}/ark/tools/color-table.html");
            ctx.RedirectPage("creature-stats", $"{root}/ark/tools/creature-stats.html");
            ctx.RedirectPage("legacy", "stats", $"{root}/ark/tools/legacy/stats.html");
        }
    }
}