namespace Orchestra.Examples.Ribbon.Changelog.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Orchestra.Changelog;

    public class ChangelogProvider : ChangelogProviderBase
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
                    Group = "Bugs",
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

            for (var i = 0; i < 100; i++)
            {
                items.Add(new ChangelogItem
                {
                    Group = "Special group",
                    Name = $"Change example {i + 1}",
                    Type = ChangelogType.Change
                });
            }

            return items;
        }
    }
}
