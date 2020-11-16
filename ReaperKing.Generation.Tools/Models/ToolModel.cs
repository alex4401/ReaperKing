using ReaperKing.Core;

namespace ReaperKing.Generation.Tools.Models
{
    public class ToolModel : BaseModel
    {
        public ToolModel(SiteContext ctx) : base(ctx)
        { }
        
        public string SiteName { get; set; }
        public string DisplayTitle { get; set; }
    }
}