using System.Text.Json.Serialization;

using Noglin.Core;

namespace Noglin.Ark.Schemas
{
    [NoglinJson(ApiVersion.External.NpcSpawningManagers, 1)]
    public record RawNpcSpawningManagers : JsonPackageSchema
    {
	    public string PersistentLevel { get; init; }

	    [JsonPropertyName("spawns")]
	    public NpcManager[] Managers { get; init; }
    }

    public record NpcManager
    {
	    [JsonPropertyName("spawnGroup")]  public string ContainerRef { get; init; }
	    [JsonPropertyName("minDesiredNumberOfNPC")] public int DesiredNumber { get; init; }
	    public bool ForceUntameable { get; init; }
	    [JsonPropertyName("locations")] public Box[] CountingLocations { get; init; }
	    public Box[] SpawnLocations { get; init; }
    }
}