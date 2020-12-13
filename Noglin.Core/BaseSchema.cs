using System.ComponentModel.DataAnnotations;

namespace Noglin.Core
{
    public abstract record PackageSchema
    {
        public string ApiVersion { get; init; }
    }
    
    public abstract record JsonPackageSchema
    {
        [Required] public string Version { get; init; }
        [Required] public string Format { get; init; }
        public PackageModInfo Mod { get; init; }
    }

    public record PackageModInfo
    {
        public string Id { get; init; }
        public string Tag { get; init; }
        public string Title { get; init; }
    }
}