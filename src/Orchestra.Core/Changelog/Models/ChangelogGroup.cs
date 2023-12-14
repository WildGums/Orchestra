namespace Orchestra.Changelog
{
    using System.Collections.Generic;

    public class ChangelogGroup
    {
        public ChangelogGroup()
        {
            Name = string.Empty;
            Items = new List<ChangelogItem>();
        }

        public string Name { get; set; }

        public List<ChangelogItem> Items { get; private set; }
    }
}
