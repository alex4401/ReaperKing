namespace ReaperKing.Core
{
    public struct PageGenerationResult
    {
        public string Extension { get; set; }
        public string Uri { get; init; }
        public string Name { get; init; }
        public bool SkipInSitemap { get; set; }
        
        public string Text { get; init; }
        public string Template { get; init; }
        public object Model { get; init; }
    }
}