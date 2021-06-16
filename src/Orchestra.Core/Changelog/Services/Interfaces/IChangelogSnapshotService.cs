namespace Orchestra.Changelog
{
    using System.Threading.Tasks;

    public interface IChangelogSnapshotService
    {
        Task<Changelog> DeserializeSnapshotAsync();
        Task SerializeSnapshotAsync(Changelog snapshot);
    }
}
