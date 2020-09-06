using System.Collections.Generic;
using ProjectReaperKing.Data.ARK;
using SiteBuilder.Core;

namespace ProjectReaperKing.Models
{
    public struct ModHomeModel
    {
        public BaseModel Super;
        
        public WebCommonModel Web;
        public string BaseUri;
        
        public ModInfo ModInfo;
        public Dictionary<string, MapInfo> Maps;
    }
}