using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class VexApplication : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult
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