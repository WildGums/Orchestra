namespace Orchestra.Changelog
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class ChangelogProviderBase : IChangelogProvider
    {
        public abstract Task<List<ChangelogItem>> GetChangelogAsync();
    }
}
