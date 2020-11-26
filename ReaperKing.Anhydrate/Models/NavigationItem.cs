namespace ReaperKing.Anhydrate.Models
{
    public readonly struct NavigationItem
    {
        public string Label { get; }
        public string Destination { get; }
        public bool IsEnabled { get; }

        public NavigationItem(string label, string destination, bool enabled = true)
        {
            Label = label;
            Destination = destination;
            IsEnabled = enabled;
        }
    }
}