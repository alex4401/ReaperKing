using ReaperKing.Core;
using ReaperKing.Generation.Tools.Models;

namespace ReaperKing.Generation.Tools
{
    public class DvJsonFilterGenerator : IPageGenerator
    {
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult
            {
                Name = "dv-json",
                Template = "wiki/dvJson.cshtml",
                Model = new ToolModel(ctx)
                {
                    SiteName = "Wiki Tools",
                    DisplayTitle = "{{Dv}}",
                },
            };
        }
    }
}