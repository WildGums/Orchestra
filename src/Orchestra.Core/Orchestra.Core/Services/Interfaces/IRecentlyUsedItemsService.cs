// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRecentlyUsedItemService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IRecentlyUsedItemsService
    {
        /// <summary>
        /// Gets or sets the maximum item count.
        /// <para />
        /// The default value is <c>10</c>.
        /// </summary>
        /// <value>The maximum item count.</value>
        int MaximumItemCount { get; set; }

        /// <summary>
        /// Gets the recently used items.
        /// </summary>
        /// <value>The items.</value>
        IEnumerable<RecentlyUsedItem> Items { get; }

        /// <summary>
        /// Gets the pinned items.
        /// </summary>
        /// <value>The items.</value>
        IEnumerable<RecentlyUsedItem> PinnedItems { get; }

        /// <summary>
        /// Adds the item to the list of recently used items.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is <c>null</c>.</exception>
        void AddItem(RecentlyUsedItem item);

        /// <summary>
        /// Occurs when the <see cref="RecentlyUsedItemsService.Items"/> property has been updated.
        /// </summary>
        event EventHandler<EventArgs> Updated;

        /// <summary>
        /// Pins the item with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        void PinItem(string name);

        /// <summary>
        /// Unpins the item with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        void UnpinItem(string name);

        /// <summary>
        /// Removes the item from the list of recently used items.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is <c>null</c>.</exception>
        void RemoveItem(RecentlyUsedItem item);
    }
}