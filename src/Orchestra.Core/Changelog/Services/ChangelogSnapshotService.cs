namespace Orchestra.Changelog
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.Services;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Orc.FileSystem;

    public class ChangelogSnapshotService : IChangelogSnapshotService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly IAppDataService _appDataService;

        public ChangelogSnapshotService(IDirectoryService directoryService, IFileService fileService,
            IAppDataService appDataService)
        {
            ArgumentNullException.ThrowIfNull(directoryService);
            ArgumentNullException.ThrowIfNull(fileService);
            ArgumentNullException.ThrowIfNull(appDataService);

            _directoryService = directoryService;
            _fileService = fileService;
            _appDataService = appDataService;
        }

        public virtual async Task SerializeSnapshotAsync(Changelog changelog)
        {
            ArgumentNullException.ThrowIfNull(changelog);

            var fileName = GetFilename();

            Log.Debug($"Serializing changelog snapshot to '{fileName}'");

            var json = JsonConvert.SerializeObject(changelog, GetSerializerSettings());

            await _fileService.WriteAllTextAsync(fileName, json);
        }

        public virtual async Task<Changelog> DeserializeSnapshotAsync()
        {
            var snapshot = new Changelog();

            var fileName = GetFilename();

            Log.Debug($"Deserializing changelog snapshot from '{fileName}'");

            if (!_fileService.Exists(fileName))
            {
                return new Changelog();
            }

            var json = await _fileService.ReadAllTextAsync(fileName);
            JsonConvert.PopulateObject(json, snapshot, GetSerializerSettings());

            return snapshot;
        }

        protected virtual string GetFilename()
        {
            var rootDirectory = _appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming);
            var changelogDirectory = Path.Combine(rootDirectory, "changelog");

            _directoryService.Create(changelogDirectory);

            return Path.Combine(changelogDirectory, "changelog.json");
        }

        protected virtual JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }
    }
}
