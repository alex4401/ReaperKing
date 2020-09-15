using System.Collections.Generic;
using System.Linq;
using ProjectReaperKing.Data.ARK;

namespace ProjectReaperKing.Data
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