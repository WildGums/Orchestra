// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecentlyUsedItemService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Catel;
    using Catel.Logging;
    using Catel.Runtime.Serialization.Xml;
    using Orchestra.Models;
    using Path = Catel.IO.Path;

    public class RecentlyUsedItemsService : IRecentlyUsedItemsService
    {
        private readonly IXmlSerializer _xmlSerializer;
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly string _fileName;
        private readonly RecentlyUsedItems _items;

        public RecentlyUsedItemsService(IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => xmlSerializer);

            _xmlSerializer = xmlSerializer;

            _fileName = Path.Combine(Path.GetApplicationDataDirectory(), "recentlyused.xml");
            _items = new RecentlyUsedItems();

            MaximumItemCount = 10;

            Load();
        }

        /// <summary>
        /// Gets or sets the maximum item count.
        /// <para />
        /// The default value is <c>10</c>.
        /// </summary>
        /// <value>The maximum item count.</value>
        public int MaximumItemCount { get; set; }

        /// <summary>
        /// Gets the recently used items.
        /// </summary>
        /// <value>The items.</value>
        public IEnumerable<RecentlyUsedItem> Items
        {
            get { return _items.Items; }
        }

        /// <summary>
        /// Gets the pinned items.
        /// </summary>
        /// <value>The items.</value>
        public IEnumerable<RecentlyUsedItem> PinnedItems
        {
            get { return _items.PinnedItems; }
        }

        /// <summary>
        /// Occurs when the <see cref="Items"/> property has been updated.
        /// </summary>
        public event EventHandler<EventArgs> Updated;

        /// <summary>
        /// Adds the item to the list of recently used items.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is <c>null</c>.</exception>
        public void AddItem(RecentlyUsedItem item)
        {
            Argument.IsNotNull(() => item);

            Log.Debug("Adding new item '{0}' to the list of recently used items", item.Name);

            if (IsAvailableInCollection(_items.PinnedItems, item.Name))
            {
                Log.Info("Item '{0}' is pinned, no need to add it to list of recently used items", item.Name);
                return;
            }

            AddSorted(_items.Items, item);

            Updated.SafeInvoke(this);

            Save();
        }

        /// <summary>
        /// Removes the item from the list of recently used items.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="item"/> is <c>null</c>.</exception>
        public void RemoveItem(RecentlyUsedItem item)
        {
            Argument.IsNotNull(() => item);

            Log.Debug("Removing item '{0}' to the list of recently used items", item.Name);

            RemoveItemFromCollection(_items.PinnedItems, item.Name);
            RemoveItemFromCollection(_items.Items, item.Name);

            Updated.SafeInvoke(this);

            Save();
        }

        private void RemoveItemFromCollection(List<RecentlyUsedItem> collection, string name)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                if (string.Equals(collection[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    Log.Debug("Found item '{0}' in the list of items, removing it", name);

                    collection.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Pins the item with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        public void PinItem(string name)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            Log.Debug("Pinning item '{0}'", name);

            for (var i = 0; i < _items.Items.Count; i++)
            {
                if (string.Equals(_items.Items[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    AddSorted(_items.PinnedItems, _items.Items[i]);
                    _items.Items.RemoveAt(i);
                    break;
                }
            }

            Updated.SafeInvoke(this);

            Save();
        }

        /// <summary>
        /// Unpins the item with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        public void UnpinItem(string name)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            Log.Debug("Unpinning item '{0}'", name);

            for (var i = 0; i < _items.PinnedItems.Count; i++)
            {
                if (string.Equals(_items.PinnedItems[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    AddSorted(_items.Items, _items.PinnedItems[i]);
                    _items.PinnedItems.RemoveAt(i);
                    break;
                }
            }

            Updated.SafeInvoke(this);

            Save();
        }

        private bool IsAvailableInCollection(List<RecentlyUsedItem> collection, string name)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                if (string.Equals(collection[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void AddSorted(List<RecentlyUsedItem> collection, RecentlyUsedItem item)
        {
            for (var i = 0; i < collection.Count; i++)
            {
                if (string.Equals(collection[i].Name, item.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Log.Debug("Found item '{0}' in the list of items, removing it and adding it to top", item.Name);

                    collection.RemoveAt(i);
                    break;
                }
            }

            var added = false;

            for (var i = 0; i < collection.Count; i++)
            {
                var collectionItem = collection[i];
                if (collectionItem.DateTime < item.DateTime)
                {
                    added = true;
                    collection.Insert(i, item);
                    break;
                }
            }

            if (!added)
            {
                collection.Add(item);
            }

            if (collection.Count > MaximumItemCount)
            {
                Log.Debug("Number of items is larger than allowed maximum of '{0}'", MaximumItemCount);

                for (int i = MaximumItemCount; i < collection.Count; i++)
                {
                    collection.RemoveAt(i);
                }
            }
        }

        private void Load()
        {
            Log.Info("Loading recently used items from '{0}'", _fileName);

            try
            {
                if (!File.Exists(_fileName))
                {
                    Log.Info("No recently used items found");
                    return;
                }

                using (var fileStream = File.Open(_fileName, FileMode.Open))
                {
                    _xmlSerializer.Deserialize(_items, fileStream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load recently used items");
            }
        }

        private void Save()
        {
            Log.Info("Saving recently used items to '{0}'", _fileName);

            try
            {
                using (var fileStream = File.Open(_fileName, FileMode.Create))
                {
                    _xmlSerializer.Serialize(_items, fileStream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save recently used items");
            }
        }
    }
}