using System;

namespace ReaperKing.Generation.ARK.Data
{
    [Obsolete("Only kept for compatibility. Use ReaperKing.Generation.ARK.CoordUtils instead.")]
    public static class CoordUtils
    {
        public static float ConvertCentimetersToGeo(float centimeters,
                                                    float worldOrigin,
                                                    float axisScale)
        {
            return ReaperKing.Generation.ARK.CoordUtils.ConvertCentimetersToGeo(centimeters, worldOrigin, axisScale);
        }

        // ReSharper disable once InconsistentNaming
        public static WorldLocation5 ConvertXYZToGeo(WorldLocation3 location,
                                                     MapInfo.GeoInfo geoInfo)
        {
            return ReaperKing.Generation.ARK.CoordUtils.ConvertXYZToGeo(location, geoInfo);
        }
    }
}