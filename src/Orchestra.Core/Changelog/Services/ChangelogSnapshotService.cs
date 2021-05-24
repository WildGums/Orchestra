namespace Orchestra.Changelog
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Services;
    using Newtonsoft.Json;
    using Orc.FileSystem;

    public class ChangelogSnapshotService : IChangelogSnapshotService
    {
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly IAppDataService _appDataService;

        public ChangelogSnapshotService(IDirectoryService directoryService, IFileService fileService,
            IAppDataService appDataService)
        {
            Argument.IsNotNull(() => directoryService);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => appDataService);

            _directoryService = directoryService;
            _fileService = fileService;
            _appDataService = appDataService;
        }

        public virtual async Task SerializeSnapshotAsync(Changelog changelog)
        {
            Argument.IsNotNull(() => changelog);

            var fileName = GetFilename();

            var json = JsonConvert.SerializeObject(changelog);

            await _fileService.WriteAllTextAsync(fileName, json);
        }

        public virtual async Task<Changelog> DeserializeSnapshotAsync()
        {
            var snapshot = new Changelog();

            var fileName = GetFilename();

            if (_fileService.Exists(fileName))
            {
                var json = await _fileService.ReadAllTextAsync(fileName);

                JsonConvert.PopulateObject(json, snapshot);
            }

            return snapshot;
        }

        protected virtual string GetFilename()
        {
            var rootDirectory = _appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming);
            var changelogDirectory = Path.Combine(rootDirectory, "changelog");

            _directoryService.Create(changelogDirectory);

            return Path.Combine(changelogDirectory, "changelog.json");
        }
    }
}
