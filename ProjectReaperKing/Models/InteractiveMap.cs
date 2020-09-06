using System.Collections.Generic;
using System.Numerics;
using ProjectReaperKing.Data.ARK;
using SiteBuilder.Core;

namespace ProjectReaperKing.Models
{
    public struct InteractiveMapModel
    {
        public BaseModel Super;
        
        public WebCommonModel Web;
        public string BaseUri;
        
        public ModInfo ModInfo;
        public MapInfo Map;
        public ModInfo.Revision Revision;
        public WorldLocation[] Nests;
        public string JsonUri;
    }
}