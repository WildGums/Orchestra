namespace Orchestra.Changelog
{
    using System;
    using System.Collections.Generic;

    public class Changelog
    {
        public Changelog()
        {
            Items = new List<ChangelogItem>();
        }

        public List<ChangelogItem> Items { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return Items.Count == 0;
            }
        }
    }
}
