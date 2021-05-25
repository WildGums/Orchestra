namespace Orchestra.Examples.Ribbon.Changelog.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Orchestra.Changelog;

    public class ChangelogProvider : Orchestra.Changelog.ChangelogProviderBase
    {
        public override async Task<IEnumerable<ChangelogItem>> GetChangelogAsync()
        {
            var items = new List<ChangelogItem>();

            items.AddRange(new[]
            {
                new ChangelogItem
                {
                    Group = "General",
                    Name = "Changelog feature",
                    Description = "The changelog feature will show a changelog of all components inside the application",
                    Type = ChangelogType.Feature
                },
                new ChangelogItem
                {
                    Group = "General",
                    Name = "Bug example",
                    Type = ChangelogType.Bug
                },
                new ChangelogItem
                {
                    Group = "General",
                    Name = "Change example",
                    Type = ChangelogType.Change
                },
                new ChangelogItem
                {
                    Group = "General",
                    Name = "Improvement example",
                    Type = ChangelogType.Improvement
                }
            });

            for (int i = 0; i < 100; i++)
            {
                items.Add(new ChangelogItem
                {
                    Group = "General",
                    Name = $"Change example {i + 1}",
                    Type = ChangelogType.Change
                });
            }

            return items;
        }
    }
}
