namespace Noglin.Ark
{
    public record ModMetadataInfo
    {
        public string Tag { get; init; }
        public string Name { get; init; }
        public ulong WorkshopId { get; init; }
    }
}