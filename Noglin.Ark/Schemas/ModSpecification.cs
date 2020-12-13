using Noglin.Core;

namespace Noglin.Ark.Schemas
{
    [NoglinYaml(Noglin.Ark.ApiVersion.V1.ModSpecification)]
    public record ModSpecificationSchema : PackageSchema
    {
        public ModMetadataInfo Meta { get; init; }
        public GitRepository Git { get; init; }
        public ModGenerationConfiguration Generation { get; init; }
        public DataMap[] DataMaps { get; init; }
    }
}