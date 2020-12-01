using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class LegacyCreatureStats : IDocumentGenerator
    {
        public DocumentGenerationResult Generate(SiteContext ctx)
        {
            return new()
            {
                Uri = "legacy",
                Name = "stats",
                Template = "/ARKTools/legacyCreatureStats.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "Legacy Tools",
                    DisplayTitle = "~alex/stats.html",
                },
            };
        }
    }
}