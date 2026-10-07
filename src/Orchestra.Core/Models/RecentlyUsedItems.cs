namespace Orchestra;

using System.Collections.Generic;
using Catel.Data;

public class RecentlyUsedItems : ModelBase
{
    public RecentlyUsedItems()
    {
        Items = new List<RecentlyUsedItem>();
        PinnedItems = new List<RecentlyUsedItem>();
    }

    public List<RecentlyUsedItem> Items { get; init; }

    public List<RecentlyUsedItem> PinnedItems { get; init; }
}
