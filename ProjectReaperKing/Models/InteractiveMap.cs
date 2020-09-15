using System;
using System.Collections.Generic;
using System.Numerics;
using ProjectReaperKing.Data.ARK;
using SiteBuilder.Core;

namespace ProjectReaperKing.Models
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