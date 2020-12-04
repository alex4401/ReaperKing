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

using System.Collections.Generic;
using System.Linq;

namespace ReaperKing.Generation.ARK.Data
{
    public partial class DataManagerARK
    {
        public IEnumerable<WorldLocation5> GetNestLocations(string modId, string mapId)
        {
            var tags = new[]
            {
                RevisionTag.ModUpdate,
                RevisionTag.ModInitDataUpdate,
            };
            var revision = FindModRevisionsByTags(modId, tags).Last();
            var map = LoadedMaps[mapId];
            var mapMainLevel = map.PersistentLevel;
            
            var liveNestsSet = revision.Item2.InitData.LiveNestSpotDefinitions;
            foreach (var nestSet in liveNestsSet)
            {
                if (nestSet.Level == mapMainLevel)
                {
                    foreach (var location in nestSet.Locations)
                    {
                        yield return CoordUtils.ConvertXYZToGeo(location, map.Geo);
                    }
                }
            }
        }
    }
}