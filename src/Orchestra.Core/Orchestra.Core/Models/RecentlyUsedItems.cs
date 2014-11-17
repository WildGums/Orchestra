// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecentlyUsedItems.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System.Collections.Generic;
    using Catel.Data;

    public class RecentlyUsedItems : ModelBase
    {
        public RecentlyUsedItems()
        {
            Items = new List<RecentlyUsedItem>();
            PinnedItems = new List<RecentlyUsedItem>();
        }

        public List<RecentlyUsedItem> Items { get; private set; }

        public List<RecentlyUsedItem> PinnedItems { get; private set; }
    }
}