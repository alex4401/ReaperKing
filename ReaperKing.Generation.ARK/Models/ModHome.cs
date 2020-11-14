using System;
using System.Collections.Generic;

using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK.Models
{
    public struct ModHomeModel
    {
        public BaseModel Super;

        public ModInfo ModInfo;
        public Dictionary<string, MapInfo> Maps;
    }
}