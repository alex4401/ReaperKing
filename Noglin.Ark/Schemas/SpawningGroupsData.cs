using System.Text.Json.Serialization;

using Noglin.Core;

namespace Noglin.Ark.Schemas
{
    [NoglinJson(ApiVersion.External.SpawningGroups, 4)]
    public record RawSpawningGroupsData : JsonPackageSchema
    {
	    // ReSharper disable once StringLiteralTypo
	    [JsonPropertyName("spawngroups")]
	    public SpawningGroupContainer[] SpawningGroups { get; init; }

	    [JsonPropertyName("externalGroupChanges")]
	    public ExtraSpawningGroupContainer[] Extra { get; init; }
    }

    public record BaseSpawningGroupContainer
    {
	    [JsonPropertyName("bp")] public string BlueprintPath { get; init; }
	    public NpcGroupInfo[] Entries { get; init; }
	    public NpcLimitInfo[] Limits { get; init; }
    }

    public sealed record SpawningGroupContainer : BaseSpawningGroupContainer
    {
	    [JsonPropertyName("maxNPCNumberMultiplier")] public float MaxNumberMultiplier { get; init; }
    }

    public sealed record ExtraSpawningGroupContainer : BaseSpawningGroupContainer
    { }

    public sealed record NpcGroupInfo
    {
	    public string Name { get; init; }
	    public float Weight { get; init; }
	    public NpcInfo[] Species { get; init; }
    }

    public sealed record NpcLimitInfo
    {
	    [JsonPropertyName("bp")] public string BlueprintPath { get; init; }
	    [JsonPropertyName("mult")] public float Multiplier { get; init; }
    }

    public sealed record NpcInfo
    {
	    public float Chance { get; init; }
	    [JsonPropertyName("bp")] public string BlueprintPath { get; init; }
	    public Vector Offset { get; init; }
    }
}