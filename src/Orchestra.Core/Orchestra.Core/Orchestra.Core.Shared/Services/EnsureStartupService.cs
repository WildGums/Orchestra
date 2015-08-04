// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnsureStartupService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using Views;

    public class EnsureStartupService : IEnsureStartupService
    {
        #region Fields
        private readonly IAppDataService _appDataService;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public EnsureStartupService(IAppDataService appDataService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => appDataService);
            Argument.IsNotNull(() => uiVisualizerService);

            _appDataService = appDataService;
            _uiVisualizerService = uiVisualizerService;

            var checkFile = GetCheckFileName();
            SuccessfullyStarted = !File.Exists(checkFile);
            
            // Always create the file
            Log.Debug("Creating fail safe file check");

            using (File.Create(checkFile))
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

            if (File.Exists(checkFile))
            {
                File.Delete(checkFile);
            }
        }

        public virtual void EnsureFailSafeStartup()
        {
            if (SuccessfullyStarted)
            {
                Log.Debug("Application was successfully started previously, starting application in normal mode");
                return;
            }

            Log.Info("Application was not successfully started previously, starting application in fail-safe mode");

            Log.Debug("Showing CrashWarningWindow dialog");

            _uiVisualizerService.ShowDialog<CrashWarningViewModel>();
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