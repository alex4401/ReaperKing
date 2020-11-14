using ReaperKing.Core;

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
                Model = new 
                {
                    Super = ctx.AcquireBaseModel(SiteName: "Wiki Tools",
                                                 DisplayTitle: "{{CreatureStats}}"),
                },
            };
        }
    }
}