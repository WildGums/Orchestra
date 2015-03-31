// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnsureStartupService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Services;

    public class EnsureStartupService : IEnsureStartupService
    {
        #region Constants
        private const string EnsureStartupCheckFile = "startupfailed.txt";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly IAppDataService _appDataService;
        private readonly ISaveFileService _saveFileService;
        #endregion

        #region Constructors
        public EnsureStartupService(ISaveFileService saveFileService, IMessageService messageService, INavigationService navigationService,
            IAppDataService appDataService)
        {
            Argument.IsNotNull(() => saveFileService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => appDataService);

            _saveFileService = saveFileService;
            _messageService = messageService;
            _navigationService = navigationService;
            _appDataService = appDataService;

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

        public virtual async Task EnsureFailSafeStartup()
        {
            if (SuccessfullyStarted)
            {
                Log.Debug("Application was successfully started previously, starting application in normal mode");
                return;
            }

            Log.Info("Application was not successfully started previously, starting application in fail-safe mode");

            var assembly = AssemblyHelper.GetEntryAssembly();

            Log.Debug("Asking user whether he/she wants to create a backup of the user data");

            switch (await _messageService.Show("It seems that the application failed to start correctly the last time it was started. In order to fix this problem, the software will remove the user data settings. It is recommended that you create a backup. Would you like to create a backup now?", assembly.Title(), MessageButton.YesNoCancel, MessageImage.Warning))
            {
                case MessageResult.Cancel:
                    Log.Info("User choose NOT to delete any data and continue (living on the edge)");
                    return;

                case MessageResult.Yes:
                    Log.Info("User choose to create a backup");

                    if (!await _appDataService.BackupUserData())
                    {
                        Log.Warning("User canceled the backup, exit application");

                        await _messageService.ShowError("Failed to created a backup. To prevent data loss, the application will now exit and not delete any files. Please contact support so they can guide you through the process.", assembly.Title());

                        _navigationService.CloseApplication();

                        return;
                    }
                    break;

                case MessageResult.No:
                    Log.Info("User choose NOT to create a backup");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            _appDataService.DeleteUserData();
        }
        #endregion

        #region Methods
        private string GetCheckFileName()
        {
            var applicationDataDirectory = _appDataService.ApplicationDataDirectory;
            var checkFile = Path.Combine(applicationDataDirectory, EnsureStartupCheckFile);

            return checkFile;
        }
        #endregion
    }
}