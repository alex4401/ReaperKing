using System;
using System.Collections.Generic;
using ReaperKing.Core;
using ReaperKing.StaticConfig.Data.ARK;

namespace ReaperKing.StaticConfig.Models
{
    public struct ModHomeModel
    {
        public BaseModel Super;

        public ModInfo ModInfo;
        public Dictionary<string, MapInfo> Maps;
    }
}