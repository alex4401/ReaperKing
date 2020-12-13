using Noglin.Core;

namespace Noglin.Ark.Schemas
{
    [NoglinYaml(Noglin.Ark.ApiVersion.V1.ProcessorConfiguration)]
    public record ProcessorConfigurationSchema : PackageSchema
    {
        public string[] OfficialMods { get; init; }
        public string[] ExcludeFromInis { get; init; }
    }
}