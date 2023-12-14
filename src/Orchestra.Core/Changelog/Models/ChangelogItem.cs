namespace Orchestra.Changelog
{
    public class ChangelogItem
    {
        public ChangelogItem()
        {
            Group = string.Empty;
            Name = string.Empty;
            Description = string.Empty;

            Type = ChangelogType.Change;
        }

        public string Group { get; set; }

        public ChangelogType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public object? Tag { get; set; }

        public override string ToString()
        {
            return $"[{Group}] {Name}";
        }
    }
}
