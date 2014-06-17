// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecentlyUsedItem.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using System;
    using Catel;
    using Catel.Data;

    public class RecentlyUsedItem : ModelBase
    {
        public RecentlyUsedItem()
        {
        }

        public RecentlyUsedItem(string name, DateTime dateTime)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            Name = name;
            DateTime = dateTime;
        }

        public string Name { get; private set; }

        public DateTime DateTime { get; private set; }
    }
}