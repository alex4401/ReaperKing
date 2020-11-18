using System.IO;
using System.Linq;

using ReaperKing.Core;
using ReaperKing.Core.Plugins;
using ReaperKing.Generation.ARK.Data;

namespace ReaperKing.Generation.ARK
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
            // Acquire a sitemap exclusion token (temporary state
            // lock) if mod is unlisted from search engines.
            SitemapLocalExclusion? sitemapLock = null;
            if (Info.ExcludeFromSitemaps)
            {
                sitemapLock = ctx.OverrideSitemaps(false);
            }
            
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

                if (Info.WithEpicIni && ctx.IsConstantDefined(StaticSwitchesArk.EpicIni))
                {
                    var egs = new EpicIniGenerator(Info, updates.Last().Item2);
                    ctx.BuildPage(egs);
                }
            }

            // Release the sitemap lock if one was acquired.
            sitemapLock?.Dispose();
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