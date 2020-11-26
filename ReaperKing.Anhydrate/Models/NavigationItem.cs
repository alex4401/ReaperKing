namespace ReaperKing.Anhydrate.Models
{
    public readonly struct NavigationItem
    {
        public readonly string Label { get; }
        public readonly string Destination { get; }
        public readonly bool IsEnabled { get; }
        public readonly bool IsCurrentRoute { get; }

        public NavigationItem(string label, string destination, bool enabled = true, bool current = false)
            => (Label, Destination, IsEnabled, IsCurrentRoute) = (label, destination, enabled, current);
    }
}