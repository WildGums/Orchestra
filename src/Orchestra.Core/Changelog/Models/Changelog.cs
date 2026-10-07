namespace Orchestra.Changelog;

using System.Collections.Generic;

public class Changelog
{
    public Changelog()
    {
        Title = string.Empty;
        Items = new List<ChangelogItem>();
    }

    public string Title { get; set; }

    public List<ChangelogItem> Items { get; init; }

    public bool IsEmpty => Items.Count == 0;
}
