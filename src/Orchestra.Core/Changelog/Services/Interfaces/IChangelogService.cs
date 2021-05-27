namespace Orchestra.Changelog
{
    using System.Threading.Tasks;

    public interface IChangelogService
    {
        Task<Changelog> GetChangelogSinceSnapshotAsync();
        Task<Changelog> GetChangelogAsync();
    }
}
