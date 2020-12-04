/*!
 * This file is a part of the open-sourced engine modules for
 * https://alex4401.github.io, and those modules' repository may be found
 * at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it
 * under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK
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
        public static WorldLocation5 ConvertXYZToGeo(WorldLocation3 location, MapInfo.GeoInfo geoInfo)
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