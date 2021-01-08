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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ReaperKing.Anhydrate.Models;

using Noglin.Ark;
using Noglin.Ark.Schemas;
using Poglin.Generation.ARK.Models;

namespace Poglin.Generation.ARK
{
    public class StandaloneIniGenerator : ModDocument<StandaloneIniModel>
    {
        private ProcessorConfigurationSchema GenerationConfig { get;  }
        private RawSpawningGroupsData SpawningGroups { get; init; }

        public StandaloneIniGenerator(ModSpecificationSchema info, ProcessorConfigurationSchema generationConfig,
                                      RawSpawningGroupsData data)
            : base(info)
        {
            GenerationConfig = generationConfig;
            SpawningGroups = data;
        }

        public override string GetName() => "ini";
        public override string GetTemplateName() => "/ARKMods/Ini";

        public override FooterInfo GetFooter()
            => base.GetFooter() with {
                Paragraphs = new[]
                {
                    "This site is not affiliated with ARK: Survival Evolved or Wildcard Properties, LLC.",
                },
            };

        public override StandaloneIniModel GetModel()
            => new(Context)
            {
                SectionName = Mod.Meta.Name,
                DocumentTitle = "Standalone INI",
                HeaderIconClass = "icon-mod",

                ModInfo = Mod,
                IniContents = GenerateIni(),
            };

        private string GenerateIni()
        {
            StringBuilder builder = new();
            builder.AppendLine("[/script/shootergame.shootergamemode]");

            foreach (ExtraSpawningGroupContainer container in SpawningGroups.Extra)
            {
                if (container.Entries.Length < 1)
                {
                    continue;
                }

                List<string> entries = new();
                List<string> limits = new();

                foreach (NpcGroupInfo group in container.Entries)
                {
                    StringBuilder entryBuilder = new();
                    List<string> npcClasses = new();
                    List<float> npcChances = new();

                    foreach (NpcInfo npc in group.Species)
                    {
                        if (GenerationConfig.ExcludeFromInis.Contains(npc.BlueprintPath.GetArkClassName()))
                        {
                            continue;
                        }
                        
                        npcClasses.Add($"\"{npc.BlueprintPath.GetArkClassName()}\"");
                        npcChances.Add(npc.Chance);
                    }

                    entryBuilder.Append($"(AnEntryName=\"{group.Name}\",");
                    entryBuilder.Append($"EntryWeight={group.Weight},");
                    entryBuilder.Append("NPCsToSpawnStrings=(");
                    entryBuilder.Append(String.Join(',', npcClasses));
                    entryBuilder.Append("),");
                    entryBuilder.Append("NPCsToSpawnPercentageChance=(");
                    entryBuilder.Append(String.Join(',', npcChances));
                    entryBuilder.Append(")");

                    entries.Add(entryBuilder.ToString());
                }

                foreach (NpcLimitInfo limit in container.Limits)
                {
                    if (GenerationConfig.ExcludeFromInis.Contains(limit.BlueprintPath.GetArkClassName()))
                    {
                        continue;
                    }
                
                    StringBuilder entryBuilder = new();
                    
                    entryBuilder.Append("(");
                    entryBuilder.Append($"NPCClassString=\"{limit.BlueprintPath.GetArkClassName()}\",");
                    entryBuilder.Append($"MaxPercentageOfDesiredNumToAllow={limit.Multiplier}");
                    entryBuilder.Append(")");
                    
                    limits.Add(entryBuilder.ToString());
                }

                builder.Append("ConfigAddNPCSpawnEntriesContainer=(");
                builder.Append($"NPCSpawnEntriesContainerClassString=\"{container.BlueprintPath.GetArkClassName()}\",");

                builder.Append("NPCSpawnEntries=(");
                builder.Append(String.Join(',', entries));
                builder.Append(")");

                if (container.Limits.Length > 0)
                {
                    builder.Append(",NPCSpawnLimits=(");
                    builder.Append(String.Join(',', limits));
                    builder.Append(")");
                }

                builder.AppendLine(")");
            }

            return builder.ToString();
        }
    }
}