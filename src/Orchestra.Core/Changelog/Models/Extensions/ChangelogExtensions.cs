namespace Orchestra.Changelog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;

    public static class ChangelogExtensions
    {
        public static Changelog GetDelta(this Changelog changelog1, Changelog changelog2)
        {
            ArgumentNullException.ThrowIfNull(changelog1);
            ArgumentNullException.ThrowIfNull(changelog2);

            // Note: we do simple delta comparsion, only the ones we added should be part

            var delta = new Changelog();

            foreach (var item in changelog2.Items)
            {
                if (changelog1.Items.Any(x => x.Name.Equals(item.Name)))
                {
                    continue;
                }

                delta.Items.Add(item);
            }

            return delta;
        }

        public static List<ChangelogGroup> CreateGroups(this Changelog changelog)
        {
            ArgumentNullException.ThrowIfNull(changelog);

            var groups = new Dictionary<string, ChangelogGroup>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in changelog.Items)
            {
                var groupName = item.Group ?? string.Empty;

                if (!groups.TryGetValue(groupName, out var group))
                {
                    group = new ChangelogGroup
                    {
                        Name = groupName
                    };

                    groups[groupName] = group;
                }

                group.Items.Add(item);
            }

            return groups.Values.OrderBy(x => x.Name).ToList();
        }
    }
}
