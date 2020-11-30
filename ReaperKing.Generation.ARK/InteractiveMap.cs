using System.IO;
using System.Linq;
using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Generation.ARK.Models;

namespace ReaperKing.Generation.ARK
{
    public class InteractiveMapGenerator : ModDocument<InteractiveMapModel>
    {
        private readonly string _arkModRef;
        private readonly string _arkMapRef;
        private readonly MapInfo _arkMap;
        private readonly int _revisionId;

        public InteractiveMapGenerator(ModInfo arkMod, string arkModRef, string arkMapRef, int revisionId)
            : base(arkMod)
        {
            _arkModRef = arkModRef;
            _arkMapRef = arkMapRef;
            _arkMap = DataManagerARK.Instance.LoadedMaps[arkMapRef];
            _revisionId = revisionId;
        }

        public override string GetName() => _arkMap.InternalId;
        public override string GetTemplateName() => "/ARKMods/SpawningMap";
        public override string GetUri()
            => "latest";
        public override FooterInfo GetFooter()
            => base.GetFooter() with {
                    Paragraphs = new[]
                    {
                        @"The topological map comes from " +
                        @"<a href=""https://ark.gamepedia.com"">the Official ARK Wiki</a>",
                    },
                };

        private string _copyDataBlobs(Site site)
        {
            var revision = Mod.Revisions[_revisionId];

            if (revision.Tag == RevisionTag.ModUpdateLegacy)
            {
                var pathOnDisk = Path.Join("data/ark", revision.PathOnDisk, _arkMap.InternalId + ".json");
                var versionHash = HashUtils.GetHashOfFile(pathOnDisk).Substring(0, 12);
                var filename = $"{Mod.InternalId}-{_arkMap.InternalId}-{versionHash}.json";
                var uri = Path.Join("data/ark", filename);
                site.CopyFileToLocation(pathOnDisk, uri);
                return filename;
            }

            return "TODO";
        }
        
        public override InteractiveMapModel GetModel() => new(Context)
        {
            SectionName = Mod.Name,
            DocumentTitle = $"{_arkMap.Name}, spawn map",
            HeaderIconClass = "icon-mod",

            ModInfo = Mod,
            Map = _arkMap,
            Revision = Mod.Revisions[_revisionId],
            Nests = DataManagerARK.Instance.GetNestLocations(_arkModRef, _arkMapRef).ToArray(),
            JsonUri = _copyDataBlobs(Context.Site),
        };
    }
}