using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK.Models
{
    public class InteractiveMapModel : BaseModel
    {
        public InteractiveMapModel(SiteContext ctx) : base(ctx)
        { }

        public string SiteName { get; set; }
        public string DisplayTitle { get; set; }
        public ModInfo ModInfo { get; set; }
        public MapInfo Map { get; set; }
        public ModInfo.Revision Revision { get; set; }
        public WorldLocation5[] Nests { get; set; }
        public string JsonUri { get; set; }
    }
}