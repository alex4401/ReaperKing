using System;
using System.Collections.Generic;
using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK.Models
{
    public record ModHomeModel : AnhydrateModel
    {
        [Obsolete]
        public string DisplayTitle { get; init; }
        
        public ModInfo ModInfo { get; init; }
        public Dictionary<string, MapInfo> Maps { get; init; }
        
        public ModHomeModel(SiteContext ctx)
            : base(ctx)
        { }
    }
}