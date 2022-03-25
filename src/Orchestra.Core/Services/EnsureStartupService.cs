// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnsureStartupService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using Orc.FileSystem;
    using Views;

    public class EnsureStartupService : IEnsureStartupService
    {
        #region Fields
        private readonly IAppDataService _appDataService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IFileService _fileService;

#pragma warning disable IDISP006 // Implement IDisposable.
        private Stream _fileStream;
#pragma warning restore IDISP006 // Implement IDisposable.
        #endregion

        #region Constructors
        public EnsureStartupService(IAppDataService appDataService, IUIVisualizerService uiVisualizerService, IFileService fileService)
        {
            Argument.IsNotNull(() => appDataService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => fileService);

            _appDataService = appDataService;
            _uiVisualizerService = uiVisualizerService;
            _fileService = fileService;
        }
        #endregion

        #region Constants
        private const string EnsureStartupCheckFile = "startupfailed.txt";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region IEnsureStartupService Members
        public bool SuccessfullyStarted { get; private set; }

        public virtual void ConfirmApplicationStartedSuccessfully()
        {
            Log.Debug("Confirming application started successfully, deleting fail safe file check");

            if (_fileStream is not null)
            {
                _fileStream.Dispose();

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
                Log.Debug("No check file exists, assuming app started successfully last time");

                SuccessfullyStarted = true;
            }
            else if (IsFileLocked(checkFile))
            {
                Log.Debug("Check file exists, but is locked so assuming there are multiple instances starting at the same time");

                SuccessfullyStarted = true;
                createFailSafeFile = false;
            }

            if (createFailSafeFile)
            {
                // Always create the file, but not if the file is currently locked
                Log.Debug("Creating fail safe file check for current instance");

                _fileStream?.Dispose();

                // Don't dispose yet, keep exclusive lock
                _fileStream = _fileService.Open(checkFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            }

            if (SuccessfullyStarted)
            {
                Log.Debug("Application was successfully started previously, starting application in normal mode");
                return;
            }

            Log.Info("Application was not successfully started previously, starting application in fail-safe mode");

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
        #endregion

        private string GetCheckFileName()
        {
            var applicationDataDirectory = _appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming);
            var checkFile = Path.Combine(applicationDataDirectory, EnsureStartupCheckFile);

            return checkFile;
        }
    }
}
