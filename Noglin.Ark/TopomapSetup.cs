namespace Noglin.Ark
{
    public record TopomapSetup
    {
        public string Small { get; init; }
        public string Large { get; init; }
        public float[] Borders { get; init; }
    }
}