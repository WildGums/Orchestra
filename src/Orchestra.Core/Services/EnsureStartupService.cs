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

            var checkFile = GetCheckFileName();
            SuccessfullyStarted = !_fileService.Exists(checkFile);
            
            // Always create the file
            Log.Debug("Creating fail safe file check");

            using (_fileService.Create(checkFile))
            {
                // Dispose required
            }
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

            var checkFile = GetCheckFileName();

            if (_fileService.Exists(checkFile))
            {
                _fileService.Delete(checkFile);
            }
        }

        public virtual async Task EnsureFailSafeStartupAsync()
        {
            if (SuccessfullyStarted)
            {
                Log.Debug("Application was successfully started previously, starting application in normal mode");
                return;
            }

            Log.Info("Application was not successfully started previously, starting application in fail-safe mode");

            Log.Debug("Showing CrashWarningWindow dialog");

            await _uiVisualizerService.ShowDialogAsync<CrashWarningViewModel>();
        }
        #endregion

        private string GetCheckFileName()
        {
            var applicationDataDirectory = _appDataService.ApplicationDataDirectory;
            var checkFile = Path.Combine(applicationDataDirectory, EnsureStartupCheckFile);

            return checkFile;
        }
    }
}