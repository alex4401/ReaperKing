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

using System.IO;
using System.Linq;

using ReaperKing.Core;
using ReaperKing.Generation.ARK.Data;
using ReaperKing.Plugins;

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
            BuildConfigurationArk config = ctx.GetConfiguration<BuildConfigurationArk>();
            
            // Acquire a sitemap exclusion token (temporary state
            // lock) if mod is unlisted from search engines.
            SitemapLocalExclusion? sitemapLock = null;
            if (Info.ExcludeFromSitemaps)
            {
                sitemapLock = ctx.OverrideSitemaps(false);
            }

            using (ctx.TryAddTemplateIncludeNamespace("ARKMods", "templates/Mods"))
            using (ctx.TryAddTemplateDefaultIncludePaths(new []
            {
                "templates/mods",
                Path.Join("templates/mods", ModTag),
            }))
            {
                var updates = _dataManager.FindModRevisionsByTag(ModTag, RevisionTag.ModUpdate);
                var homePage = new ModHomeGenerator(Info);

                ctx.BuildPage(homePage);
                BuildInteractiveMaps(ctx);

                if (Info.WithEpicIni && config.GenerateInis)
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
                var map = new InteractiveMapGenerator(Info, ModTag, mapRef, revisionId);
                ctx.BuildPage(map);
            }
            
            // Map out new-style revisions to worlds.
            var updates = _dataManager.FindModRevisionsByTag(ModTag, RevisionTag.ModUpdate);

        }
    }
}