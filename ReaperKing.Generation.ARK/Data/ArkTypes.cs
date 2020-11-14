using System.Collections.Generic;

namespace ReaperKing.Generation.ARK.Data
{
    public enum RevisionTag
    {
        None,
        ModUpdateLegacy,
        ModUpdate,
        ModInitDataUpdate,
    }

    public struct ModInfo
    {
        public string Name;
        public string InternalId;

        public string Git;
        public ulong SteamId;

        public bool PublicIssueTracker;
        public bool WithEpicIni;

        public List<Revision> Revisions;

        public struct Revision
        {
            public RevisionTag Tag;
            public string Date;
            public int SingletonRef;
            public string[] Contents;

            public string PathOnDisk;

            public ModInitializationData InitData;
        }
    }

    public struct MapInfo
    {
        public string Name;
        public string InternalId;
        public string PersistentLevel;

        public GeoInfo Geo;
        public TopomapInfo Topomap;

        public struct GeoInfo
        {
            public int[] Origins;
            public float[] Scales;
        }

        public struct TopomapInfo
        {
            public string Name;
            public string Version;
            public float[] Borders;
        }
    }

    public struct ModInitializationData
    {
        public NestSpotList[] LiveNestSpotDefinitions { get; set; }
    }

    public struct PrimalGameData
    {

    }

    public struct WorldLocation3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public struct WorldLocation5
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Lat;
        public float Long;
    }

    public struct NestSpotList
    {
        public string Level { get; set; }
        public WorldLocation3[] Locations { get; set; }
    }
}