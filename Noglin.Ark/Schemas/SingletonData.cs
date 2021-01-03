using System.Collections.Generic;
using System.Text.Json.Serialization;

using Noglin.Core;

namespace Noglin.Ark.Schemas
{
    [NoglinJson(ApiVersion.External.Singleton, 1)]
    public record SingletonPackage : JsonPackageSchema
    {
	    public string[] ForcedExactMatches { get; init; }
	    public string NestActorListTag { get; init; }
	    public CreatureNestInfo[] Nests { get; init; }
    }

    public record CreatureNestInfo
    {
	    public string LevelName { get; init; }
	    public Vector[] Locations { get; init; }
    }
}