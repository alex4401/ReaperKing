using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectReaperKing.Data;
using ProjectReaperKing.Data.ARK;
using SiteBuilder.Core;

namespace ProjectReaperKing.ContentGeneration.ARK
{
    public class ModContentProvider : ISiteContentProvider
    {
        public readonly string ModTag;
        public readonly ModInfo Info;
        private readonly DataManagerARK _dataManager;
        
        public ModContentProvider(string modTag)
        {
            ModTag = modTag;
            Info = DataManagerARK.Instance.LoadedMods[modTag];
            _dataManager = DataManagerARK.Instance;
        }

        public void BuildContent(SiteContext ctx)
        {
            using (ctx.AddOptionalTemplateDirectories(new []
            {
                "templates/mods",
                Path.Join("templates/mods", ModTag),
            }))
            {
                var updates = _dataManager.FindModRevisionsByTag(ModTag, RevisionTag.ModUpdate);
                var homePage = new ModHomeGenerator(Info);

                ctx.BuildPage(homePage);
                BuildInteractiveMaps(ctx);

                if (Info.WithEpicIni && ctx.IsConstantDefined(Features.EpicIni))
                {
                    var egs = new EpicIniGenerator(Info, updates.Last().Item2);
                    ctx.BuildPage(egs);
                }
            }
        }

        public void BuildInteractiveMaps(SiteContext ctx)
        {
            var worldRevMap = _dataManager.MapLegacyRevisionsToMaps(ModTag);
            
            foreach (var (mapRef, revisionId) in worldRevMap)
            {
                var map = new InteractiveMapGenerator(ModTag, mapRef, revisionId);
                ctx.BuildPage(map);
            }
            
            // Map out new-style revisions to worlds.
            var updates = _dataManager.FindModRevisionsByTag(ModTag, RevisionTag.ModUpdate);

        }
    }
}