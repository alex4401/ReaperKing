using ReaperKing.Core;

namespace ReaperKing.Generation.Tools.Models
{
    public record ToolModel : BaseModel
    {
        public string SiteName { get; init; }
        public string DisplayTitle { get; init; }
        
        public ToolModel(SiteContext ctx)
            : base(ctx)
        { }
    }
}