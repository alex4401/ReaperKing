using System;
using System.IO;
using System.Linq;
using ProjectReaperKing.Data;
using ProjectReaperKing.Data.ARK;
using ProjectReaperKing.Models;
using SiteBuilder.Core;

namespace ProjectReaperKing.Pages.ARK
{
        public class InteractiveMapGenerator : IPageGenerator
        {
            private readonly ModInfo _arkMod;
            private readonly string _arkModRef;
            private readonly string _arkMapRef;
            private readonly MapInfo _arkMap;
            private readonly int _revisionId;

            public InteractiveMapGenerator(string arkModRef, string arkMapRef, int revisionId)
            {
                _arkModRef = arkModRef;
                _arkMod = DataManagerARK.Instance.LoadedMods[arkModRef];
                _arkMapRef = arkMapRef;
                _arkMap = DataManagerARK.Instance.LoadedMaps[arkMapRef];
                _revisionId = revisionId;
            }

            public PageGenerationResult Generate(Site site, string parentUri)
            {
                return new PageGenerationResult()
                {
                    Uri = "latest",
                    Name = _arkMap.InternalId,
                    Template = "mods/interactiveMap.cshtml",
                    Model = new InteractiveMapModel
                    {
                        Super = new BaseModel(site)
                        {
                            SiteName = _arkMod.Name,
                            DisplayTitle = $"{_arkMod.Name}, interactive spawning maps",
                            RootUri = parentUri,
                        },
                        
                        ModInfo = _arkMod,
                        Map = _arkMap,
                        Revision = _arkMod.Revisions[_revisionId],
                        Nests = DataManagerARK.Instance.GetNestLocations(_arkModRef, _arkMapRef).ToArray(),
                        JsonUri = _copyDataBlobs(site),
                    },
                };
            }

            private string _copyDataBlobs(Site site)
            {
                var revision = _arkMod.Revisions[_revisionId];

                if (revision.Tag == RevisionTag.ModUpdateLegacy)
                {
                    var pathOnDisk = Path.Join("data/ark", revision.PathOnDisk, _arkMap.InternalId + ".json");
                    var versionHash = HashUtils.GetHashOfFile(pathOnDisk).Substring(0, 12);
                    var filename = $"{_arkMod.InternalId}-{_arkMap.InternalId}-{versionHash}.json";
                    var uri = Path.Join("data/ark", filename);
                    site.CopyFileToLocation(pathOnDisk, uri);
                    return filename;
                }

                return "TODO";
            }
        }
}