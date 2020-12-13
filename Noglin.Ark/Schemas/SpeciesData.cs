using System.Text.Json.Serialization;

using Noglin.Core;

namespace Noglin.Ark.Schemas
{
    [NoglinJson(ApiVersion.External.Species, 2)]
    public record CreaturesPackage : JsonPackageSchema
    {
	    // ReSharper disable once StringLiteralTypo
	    [JsonPropertyName("species")]
	    public CreatureInfo[] Creatures { get; init; }
    }

    public record CreatureInfo
    {
	    public string Name { get; init; }
	    [JsonPropertyName("bp")] public string BlueprintPath { get; init; }
	    public string[] Flags { get; init; }
    }
}