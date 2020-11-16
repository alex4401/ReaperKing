using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class LegacyCreatureStats : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult
            {
                Uri = "legacy",
                Name = "stats",
                Template = "wiki/legacyCreatureStats.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "Legacy Tools",
                    DisplayTitle = "~alex/stats.html",
                },
            };
        }
    }
}