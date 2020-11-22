using System.IO;
using ReaperKing.Core;
using ReaperKing.CommonTemplates.Extensions;

namespace ReaperKing.Generation.Tools
{
    public class ToolsRedirectsProvider : ISiteContentProvider
    {
        public void BuildContent(SiteContext ctx)
        {
            string root = ctx.Site.ProjectConfig.Paths.Root;
            
            ctx.RedirectPage("color-table", Path.Join(root, "ark/tools/color-table.html"));
            ctx.RedirectPage("creature-stats", Path.Join(root, "ark/tools/creature-stats.html"));
            ctx.RedirectPage("legacy", "stats", Path.Join(root, "ark/tools/legacy/stats.html"));
        }
    }
}