namespace ReaperKing.Anhydrate.Models
{
    public struct GutterLayoutSection
    {
        public string Id { get; init; }
        public SectionType Type { get; init; }
        public string CustomClass { get; init; }
        public string HtmlId { get; init; }

        public enum SectionType
        {
            Full,
            Grid,
            Row,
            NarrowFit,
            Narrower,
            Break,
            BreakMobile,
            Custom,
        }
    }
}