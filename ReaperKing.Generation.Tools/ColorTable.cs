using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class ColorTable : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult
            {
                Name = "color-table",
                Template = "wiki/colorTable.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "Wiki Tools",
                    DisplayTitle = "[[Color IDs]]",
                },
            };
        }
    }
}