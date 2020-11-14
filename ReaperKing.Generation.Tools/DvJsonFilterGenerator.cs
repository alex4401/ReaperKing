using ReaperKing.Core;

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
                Model = new 
                {
                    Super = ctx.AcquireBaseModel(SiteName: "Wiki Tools",
                                                 DisplayTitle: "{{dv}}"),
                },
            };
        }
    }
}