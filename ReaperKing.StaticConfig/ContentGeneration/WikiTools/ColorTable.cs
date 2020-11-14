using ReaperKing.Core;

namespace ReaperKing.StaticConfig.ContentGeneration.WikiTools
{
    public class ColorTable : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult
            {
                Name = "color-table",
                Template = "wiki/colorTable.cshtml",
                Model = new 
                {
                    Super = ctx.AcquireBaseModel(SiteName: "Wiki Tools",
                                                 DisplayTitle: "[[Color IDs]]"),
                },
            };
        }
    }
}