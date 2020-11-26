using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class CreatureStats : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new()
            {
                Name = "creature-stats",
                Template = "/ARKTools/creatureStats.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "ARK Tools",
                    DisplayTitle = "{{CreatureStats}}",
                },
            };
        }
    }
}