using Noglin.Core;

namespace Noglin.Ark.Schemas
{
    [NoglinSchema(Noglin.Ark.ApiVersion.V1.ModSpecification)]
    public record ModSpecification : PackageSchema
    {
        public ModMetadataInfo Meta { get; init; }
        public GitRepository Git { get; init; }
        public ModGenerationConfiguration Generation { get; init; }
        public DataMap[] DataMaps { get; init; }
    }
}