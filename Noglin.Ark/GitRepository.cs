namespace Noglin.Ark
{
    public record GitRepository
    {
        public string Service { get; init; }
        public string Owner { get; init; }
        public string Repository { get; init; }
    }
}