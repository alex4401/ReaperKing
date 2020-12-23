/*!
 * This file is a part of the Poglin project, whose repository may be
 * found at https://github.com/alex4401/ReaperKing.
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
using Noglin.Ark;
using Noglin.Ark.Schemas;
using Poglin.Generation.ARK.Models;
using ReaperKing.Anhydrate.Models;

namespace Poglin.Generation.ARK
{
    public class InteractiveMapGenerator : ModDocument<InteractiveMapModel>
    {
        private DataMap Map { get; init; }
        private SingletonPackage Singleton { get; init; }

        public InteractiveMapGenerator(ModSpecificationSchema mod,
                                       DataMap map,
                                       SingletonPackage singleton)
            : base(mod)
        {
            Map = map;
            Singleton = singleton;
        }

        public override string GetName() => Map.Tag;
        public override string GetTemplateName() => "/ARKMods/SpawningMap";
        public override FooterInfo GetFooter()
            => base.GetFooter() with {
                    Paragraphs = new[]
                    {
                        @"The topographic map comes from " +
                        @"<a href=""https://ark.gamepedia.com"">the Official ARK Wiki</a>",
                        "This site is not affiliated with ARK: Survival Evolved or Wildcard Properties, LLC.",
                    },
                };
        
        public override InteractiveMapModel GetModel()
            => new(Context)
            {
                SectionName = Mod.Meta.Name,
                DocumentTitle = $"{Map.Name}, spawn map",
                HeaderIconClass = "icon-mod",

                ModInfo = Mod,
                Map = Map,
            
                Nests = GetNestLocations().ToArray(),
                JsonUri = GenerateJsonBlob(),
            };

        private IEnumerable<WorldLocation> GetNestLocations()
        {
            foreach (CreatureNestInfo nests in Singleton.Nests)
            {
                if (nests.LevelName != Map.Level)
                {
                    continue;
                }

                foreach (Vector vec in nests.Locations)
                {
                    yield return new WorldLocation(vec, Map);
                }
            }
        }

        private string GenerateJsonBlob()
        {
            return "TODO";
        }
    }
}