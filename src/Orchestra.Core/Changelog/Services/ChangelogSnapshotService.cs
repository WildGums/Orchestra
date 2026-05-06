namespace Orchestra.Changelog;

using System;
using System.IO;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.Services;
using Microsoft.Extensions.Logging;
using Orc.FileSystem;
using Orc.Serialization.Json;

public class ChangelogSnapshotService : IChangelogSnapshotService
{
    private readonly ILogger<ChangelogSnapshotService> _logger;
    private readonly IDirectoryService _directoryService;
    private readonly IFileService _fileService;
    private readonly IAppDataService _appDataService;
    private readonly IJsonSerializerFactory _jsonSerializerFactory;

    public ChangelogSnapshotService(ILogger<ChangelogSnapshotService> logger, IDirectoryService directoryService,
        IFileService fileService, IAppDataService appDataService, IJsonSerializerFactory jsonSerializerFactory)
    {
        _logger = logger;
        _directoryService = directoryService;
        _fileService = fileService;
        _appDataService = appDataService;
        _jsonSerializerFactory = jsonSerializerFactory;
    }

    public virtual async Task SerializeSnapshotAsync(Changelog changelog)
    {
        ArgumentNullException.ThrowIfNull(changelog);

        var fileName = GetFilename();

        _logger.LogDebug("Serializing changelog snapshot to '{FileName}'", fileName);

        using (var fileStream = _fileService.OpenWrite(fileName))
        {
            var serializer = _jsonSerializerFactory.CreateSerializer();
            serializer.Serialize(fileStream, changelog);

            await fileStream.FlushAsync();
        }
    }

    public virtual async Task<Changelog> DeserializeSnapshotAsync()
    {
        var fileName = GetFilename();

        _logger.LogDebug("Deserializing changelog snapshot from '{FileName}'", fileName);

        if (_fileService.Exists(fileName))
        {
            using (var fileStream = _fileService.OpenRead(fileName))
            {
                var serializer = _jsonSerializerFactory.CreateSerializer();
                var changelog = serializer.Deserialize<Changelog>(fileStream);
                if (changelog is not null)
                {
                    return changelog;
                }
            }
        }

        return new Changelog();
    }

    protected virtual string GetFilename()
    {
        var rootDirectory = _appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming);
        var changelogDirectory = Path.Combine(rootDirectory, "changelog");

        _directoryService.Create(changelogDirectory);

        return Path.Combine(changelogDirectory, "changelog.json");
    }
}
