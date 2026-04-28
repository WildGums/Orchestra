namespace Orchestra.Changelog;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IChangelogProvider
{
    Task<IReadOnlyList<ChangelogItem>> GetChangelogAsync();
}
