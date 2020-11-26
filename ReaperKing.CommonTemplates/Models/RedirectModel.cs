using ReaperKing.Core;

namespace ReaperKing.CommonTemplates.Models
{
    public record RedirectModel : BaseModel
    {
        public string Target { get; init; }

        public RedirectModel(SiteContext ctx)
            : base(ctx)
        { }
    }
}