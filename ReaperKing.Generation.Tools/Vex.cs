using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class VexApplication : IDocumentGenerator
    {
        public DocumentGenerationResult Generate(SiteContext ctx)
        {
            return new()
            {
                Name = "vex",
                Template = "/ARKTools/vex.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "ARK Tools",
                    DisplayTitle = "Vex",
                },
            };
        }
    }
}