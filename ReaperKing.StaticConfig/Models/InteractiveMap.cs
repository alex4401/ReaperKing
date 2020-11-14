using System;
using System.Collections.Generic;
using System.Numerics;
using ReaperKing.Core;
using ReaperKing.StaticConfig.Data.ARK;

namespace ReaperKing.StaticConfig.Models
{
    public struct InteractiveMapModel
    {
        public BaseModel Super;

        public ModInfo ModInfo;
        public MapInfo Map;
        public ModInfo.Revision Revision;
        public WorldLocation5[] Nests;
        public string JsonUri;
    }
}