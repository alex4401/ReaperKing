namespace Noglin.Ark
{
    public record WorldLocation : Vector
    {
        public float Lat { get; init; }
        public float Long { get; init; }

        public WorldLocation(Vector vec, DataMap map)
        {
            (X, Y, Z) = (vec.X, vec.Y, vec.Z);
            Lat = CoordUtils.ConvertCentimetersToGeo(Y, map.Setup.Lat[0], map.Setup.Lat[1]);
            Long = CoordUtils.ConvertCentimetersToGeo(X, map.Setup.Long[0], map.Setup.Long[1]);
        }
    }
}