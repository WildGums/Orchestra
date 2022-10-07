namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Services;
    using Orc.FileSystem;

    public class ManageAppDataService : Orchestra.Services.IManageAppDataService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ISaveFileService _saveFileService;
        private readonly IProcessService _processService;
        private readonly IDirectoryService _directoryService;
        private readonly IFileService _fileService;
        private readonly IAppDataService _appDataService;

        public ManageAppDataService(ISaveFileService saveFileService, IProcessService processService,
            IDirectoryService directoryService, IFileService fileService, IAppDataService appDataService)
        {
            ArgumentNullException.ThrowIfNull(saveFileService);
            ArgumentNullException.ThrowIfNull(processService);
            ArgumentNullException.ThrowIfNull(directoryService);
            ArgumentNullException.ThrowIfNull(fileService);
            ArgumentNullException.ThrowIfNull(appDataService);

            _saveFileService = saveFileService;
            _processService = processService;
            _directoryService = directoryService;
            _fileService = fileService;
            _appDataService = appDataService;

            ExclusionFilters = new List<string>(new[]
            {
                "licenseinfo.xml",
                "*.log"
            });
        }

        public List<string> ExclusionFilters { get; private set; }

        public bool OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget applicationDataTarget)
        {
            Log.Info("Opening data directory");

            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(applicationDataTarget);

            _processService.StartProcess(new ProcessContext
            {
                FileName = "explorer.exe",
                Arguments = applicationDataDirectory
            });

            return true;
        }

        protected virtual bool MatchesFilters(IEnumerable<string> filters, string fileName)
        {
            return FilterHelper.MatchesFilters(filters, fileName);
        }

        public async Task DeleteUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget)
        {
            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(applicationDataTarget);

            Log.Debug("Deleting user data from '{0}'", applicationDataDirectory);

            var exclusionFilters = ExclusionFilters;

            var allFiles = _directoryService.GetFiles(applicationDataDirectory, "*.*", SearchOption.AllDirectories);
            foreach (var fileName in allFiles)
            {
                if (!MatchesFilters(exclusionFilters, fileName))
                {
                    _fileService.Delete(fileName);
                }
            }
        }

        public async Task<bool> BackupUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget)
        {
            var assembly = AssemblyHelper.GetEntryAssembly();
            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(applicationDataTarget);

            var result = await _saveFileService.DetermineFileAsync(new DetermineSaveFileContext
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = string.Format("{0} backup {1}.zip", assembly.Title(), DateTime.Now.ToString("yyyyMMdd hhmmss")),
                Filter = "Zip files|*.zip"
            });

            if (!result.Result)
            {
                return false;
            }

            var zipFileName = result.FileName;

            Log.Debug("Writing zip file to '{0}'", zipFileName);

            using (var fileStream = _fileService.Create(zipFileName))
            {
                using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    zipArchive.CreateEntryFromDirectory(applicationDataDirectory, string.Empty, CompressionLevel.Optimal);

                    await fileStream.FlushAsync();
                }
            }

            return true;
        }
    }
}
