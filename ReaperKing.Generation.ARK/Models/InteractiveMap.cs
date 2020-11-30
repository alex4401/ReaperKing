using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK.Models
{
    public record InteractiveMapModel : AnhydrateModel
    {
        public ModInfo ModInfo { get; init; }
        public MapInfo Map { get; init; }
        public ModInfo.Revision Revision { get; init; }
        public WorldLocation5[] Nests { get; init; }
        public string JsonUri { get; init; }
        
        public InteractiveMapModel(SiteContext ctx)
            : base(ctx)
        { }
    }
}