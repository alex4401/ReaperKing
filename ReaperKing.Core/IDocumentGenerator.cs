namespace ReaperKing.Core
{
    public interface IDocumentGenerator
    {
        public DocumentGenerationResult Generate(SiteContext ctx);
    }
}