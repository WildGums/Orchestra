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
            return new[]
            {
                new ChangelogItem
                {
                    Group = "General",
                    Name = "Changelog feature",
                    Description = "The changelog feature will show a changelog of all components inside the application",
                    Type = ChangelogType.Feature
                }
            };
        }
    }
}
