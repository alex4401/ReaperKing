namespace Noglin.Ark
{
    public record GitRepository
    {
        public string Service { get; init; }
        public string Owner { get; init; }
        public string Repository { get; init; }
        public bool IsPublic { get; init; }

        public string Address => $"{Service}/{Owner}/{Repository}";
        public string IssueTracker => Address + "/-/issues";
    }
}