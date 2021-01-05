/*!
 * This file is a part of the Poglin project, whose repository may be found at https://github.com/alex4401/ReaperKing.
 *
 * The project is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General
 * Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Affero General Public License along with this program. If not, see
 * https://www.gnu.org/licenses/.
 */

using System.Linq;

using ReaperKing.Core;

using Noglin.Ark;
using Noglin.Ark.Schemas;
using Noglin.Core;

namespace Poglin.Generation.ARK
{
    public class ModContentProvider : IDocumentProvider
    {
        public ArkDataContentGenerator Manager { get; init; }
        public BuildConfigurationArk Config { get; init; }
        public PackageRegistry ArkRegistry { get; init; }
        public ModSpecificationSchema Info { get; init; }
        public string Tag { get; init; }
        
        public ModContentProvider(ArkDataContentGenerator outer, ModSpecificationSchema modSpec)
        {
            Manager = outer;
            Config = outer.Config;
            ArkRegistry = outer.ArkRegistry;
            Info = modSpec;
            Tag = modSpec.Meta.Tag;
        }

        public void BuildContent(SiteContext ctx)
        {
            using TemplateNamespaceMount _ = ctx.TryAddTemplateIncludeNamespace("ARKMods", "templates/Mods");
            ctx.EmitDocument<ModHomeGenerator>(new(Info));

            if (!Info.Generation.OnlyPlaceholder)
            {
                var groups = ArkRegistry.FindByModId<RawSpawningGroupsData>(Info.Meta.WorkshopId).First();
                var singleton = ArkRegistry.FindByModId<SingletonPackage>(Info.Meta.WorkshopId).First();

                foreach (DataMap dataMap in Info.DataMaps)
                {
                    ctx.EmitDocument<InteractiveMapGenerator>(new(Info, dataMap, singleton), "/latest");
                }

                if (Config.GenerateInis && Info.Generation.GenerateInis)
                {
                    ctx.EmitDocument<StandaloneIniGenerator>(new(Info, groups), "/latest");
                }
            }

        }
    }
}