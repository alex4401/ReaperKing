using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class ColorTable : IDocumentGenerator
    {
        public DocumentGenerationResult Generate(SiteContext ctx)
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