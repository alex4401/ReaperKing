namespace Noglin.Ark
{
    public record ModGenerationConfiguration
    {
        public bool GenerateInis { get; init; } = false;
        public bool AllowSitemapping { get; init; } = true;
    }
}