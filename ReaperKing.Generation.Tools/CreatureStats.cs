using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class CreatureStats : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult
            {
                Name = "creature-stats",
                Template = "wiki/creatureStats.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "Wiki Tools",
                    DisplayTitle = "{{CreatureStats}}",
                },
            };
        }
    }
}