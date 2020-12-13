using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noglin.Ark;
using Noglin.Ark.Schemas;
using ReaperKing.Anhydrate.Models;
using ReaperKing.Core;
using ReaperKing.Generation.ARK.Models;

namespace ReaperKing.Generation.ARK
{
    public class StandaloneIniGenerator : ModDocument<StandaloneIniModel>
    {
        private RawSpawningGroupsData SpawningGroups { get; init; }

        public StandaloneIniGenerator(ModSpecificationSchema info, RawSpawningGroupsData data)
            : base(info)
        {
            SpawningGroups = data;
        }

        public override string GetName() => "ini";
        public override string GetUri() => "latest";
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