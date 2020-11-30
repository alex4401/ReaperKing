namespace ReaperKing.Anhydrate.Models
{
    public struct GutterLayoutSection
    {
        public string Id { get; init; }
        public ColumnType Type { get; init; }

        public enum ColumnType
        {
            Full,
            Grid,
        }
    }
}