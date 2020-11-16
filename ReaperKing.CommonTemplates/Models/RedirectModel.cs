using ReaperKing.Core;

namespace ReaperKing.CommonTemplates.Models
{
    public class RedirectModel : BaseModel
    {
        public RedirectModel(SiteContext ctx) : base(ctx)
        { }

        public string Target { get; set; }
    }
}