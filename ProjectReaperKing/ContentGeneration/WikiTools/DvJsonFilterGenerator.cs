using SiteBuilder.Core;

namespace ProjectReaperKing.ContentGeneration.WikiTools
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