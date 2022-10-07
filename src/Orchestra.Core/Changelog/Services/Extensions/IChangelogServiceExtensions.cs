namespace Orchestra.Changelog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;

    public static class IChangelogServiceExtensions
    {
        public static async Task<List<ChangelogItem>> GetChangelogItemsForGroupAsync(this IChangelogService changelogService,
            string groupName)
        {
            ArgumentNullException.ThrowIfNull(changelogService);

            var changelog = await changelogService.GetChangelogAsync();

            return (from x in changelog.Items
                    where x.Group.EqualsIgnoreCase(groupName)
                    select x).ToList();
        }
    }
}
