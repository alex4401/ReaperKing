using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class ColorTable : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new()
            {
                Name = "color-table",
                Template = "/ARKTools/colorTable.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "ARK Tools",
                    DisplayTitle = "[[Color IDs]]",
                },
            };
        }
    }
}