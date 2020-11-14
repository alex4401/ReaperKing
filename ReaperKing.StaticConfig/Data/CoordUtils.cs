using ReaperKing.StaticConfig.Data.ARK;

namespace ReaperKing.StaticConfig.Data
{
    public static class CoordUtils
    {
        public static float ConvertCentimetersToGeo(float centimeters,
                                                    float worldOrigin,
                                                    float axisScale)
        {
            // Reference: Purlovia, export.wiki.maps.common.get_latlong_from_location
            //                      export.wiki.maps.gathering_complex.WorldSettingsExport.extract
            return centimeters / (axisScale * 10) + (-worldOrigin / (axisScale * 10));
        }

        // ReSharper disable once InconsistentNaming
        public static WorldLocation5 ConvertXYZToGeo(WorldLocation3 location,
                                                     MapInfo.GeoInfo geoInfo)
        {
            return new WorldLocation5
            {
                X = location.X,
                Y = location.Y,
                Z = location.Z,
                Lat = ConvertCentimetersToGeo(location.Y, geoInfo.Origins[0], geoInfo.Scales[0]),
                Long = ConvertCentimetersToGeo(location.X, geoInfo.Origins[1], geoInfo.Scales[1]),
            };
        }
    }
}