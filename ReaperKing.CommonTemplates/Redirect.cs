using ReaperKing.Core;
using ReaperKing.CommonTemplates.Models;

namespace ReaperKing.CommonTemplates
{
    public class RedirectGenerator : IPageGenerator
    {
        public string Uri { get; set; }
        public string Name { get; set; }
        public string TargetUrl { get; set; }
        
        public PageGenerationResult Generate(SiteContext ctx)
        {
            return new PageGenerationResult()
            {
                Uri = Uri,
                Name = Name,
                Template = "ReaperKing.CommonTemplates/redirect.cshtml",
                Model = new RedirectModel(ctx)
                {
                    Target = TargetUrl,
                },
            };
        }
    }
}