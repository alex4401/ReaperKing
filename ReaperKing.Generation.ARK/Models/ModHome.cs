using System;
using System.Collections.Generic;

using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK.Models
{
    public class ModHomeModel : BaseModel
    {
        public ModHomeModel(SiteContext ctx) : base(ctx)
        { }

        public string SiteName { get; set; }
        public string DisplayTitle { get; set; }
        public ModInfo ModInfo { get; set; }
        public Dictionary<string, MapInfo> Maps { get; set; }
    }
}