namespace Orchestra.Changelog
{
    using System.Collections.Generic;

    public class Changelog
    {
        public Changelog(ChangelogDeltaType type = ChangelogDeltaType.Full)
        {
            Items = new List<ChangelogItem>();
            Type = type;
        }

        public string Title { get; set; }

        public List<ChangelogItem> Items { get; private set; }

        public bool IsEmpty => Items.Count == 0;

        public ChangelogDeltaType Type { get; }
    }
}
