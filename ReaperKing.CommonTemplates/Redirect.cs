using ReaperKing.Core;
using ReaperKing.CommonTemplates.Models;

namespace ReaperKing.CommonTemplates
{
    public class RedirectGenerator : IDocumentGenerator
    {
        public string Uri { get; init; }
        public string Name { get; init; }
        public string TargetUrl { get; init; }
        
        public DocumentGenerationResult Generate(SiteContext ctx)
        {
            return new()
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