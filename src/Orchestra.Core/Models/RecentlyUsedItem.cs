namespace Orchestra
{
    using System;
    using Catel;
    using Catel.Data;

    public class RecentlyUsedItem : ModelBase
    {
        public RecentlyUsedItem()
        {
            Name = string.Empty;
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
