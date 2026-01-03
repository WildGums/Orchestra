namespace Orchestra
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel.Services;
    using Microsoft.Extensions.Logging;
    using Orc.FileSystem;
    using Views;

    public class EnsureStartupService : IEnsureStartupService
    {
        private const string EnsureStartupCheckFile = "startupfailed.txt";

        private readonly ILogger<EnsureStartupService> _logger;
        private readonly IAppDataService _appDataService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IFileService _fileService;
        private readonly IDirectoryService _directoryService;

#pragma warning disable IDISP006 // Implement IDisposable.
        private Stream? _fileStream;
#pragma warning restore IDISP006 // Implement IDisposable.

        public EnsureStartupService(ILogger<EnsureStartupService> logger, IAppDataService appDataService,
            IUIVisualizerService uiVisualizerService, IFileService fileService, IDirectoryService directoryService)
        {
            _logger = logger;
            _appDataService = appDataService;
            _uiVisualizerService = uiVisualizerService;
            _fileService = fileService;
            _directoryService = directoryService;
        }

        public bool SuccessfullyStarted { get; private set; }

        public virtual async Task ConfirmApplicationStartedSuccessfullyAsync()
        {
            _logger.LogDebug("Confirming application started successfully, deleting fail safe file check");

            if (_fileStream is not null)
            {
                await _fileStream.DisposeAsync();
                _fileStream = null;

                var checkFile = GetCheckFileName();
                _fileService.Delete(checkFile);
            }
        }

        public virtual async Task EnsureFailSafeStartupAsync()
        {
            var checkFile = GetCheckFileName();
            var createFailSafeFile = true;

            // Check whether the file is locked to make sure that there is not
            // another instance currently starting ups
            var fileExists = _fileService.Exists(checkFile);
            if (!fileExists)
            {
                _logger.LogDebug("No check file exists, assuming app started successfully last time");

                SuccessfullyStarted = true;
            }
            else if (IsFileLocked(checkFile))
            {
                _logger.LogDebug("Check file exists, but is locked so assuming there are multiple instances starting at the same time");

                SuccessfullyStarted = true;
                createFailSafeFile = false;
            }

            if (createFailSafeFile)
            {
                // Always create the file, but not if the file is currently locked
                _logger.LogDebug("Creating fail safe file check for current instance");

                _fileStream?.Dispose();

                // Don't dispose yet, keep exclusive lock
                var directory = Path.GetDirectoryName(checkFile);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    _directoryService.Create(directory);

                    _fileStream = _fileService.Open(checkFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                }
            }

            if (SuccessfullyStarted)
            {
                _logger.LogDebug("Application was successfully started previously, starting application in normal mode");
                return;
            }

            _logger.LogInformation("Application was not successfully started previously, starting application in fail-safe mode");

            await _uiVisualizerService.ShowDialogAsync<CrashWarningViewModel>();
        }

        protected bool IsFileLocked(string fileName)
        {
            try
            {
                using (var stream = _fileService.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    // Empty by design
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        private string GetCheckFileName()
        {
            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming);
            var checkFile = Path.Combine(applicationDataDirectory, EnsureStartupCheckFile);

            return checkFile;
        }
    }
}
