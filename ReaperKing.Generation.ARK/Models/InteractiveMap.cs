using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK.Models
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