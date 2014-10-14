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
    using Ionic.Zip;

    public class EnsureStartupService : IEnsureStartupService
    {
        #region Constants
        private const string EnsureStartupCheckFile = "startupfailed.txt";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly ISaveFileService _saveFileService;
        #endregion

        #region Constructors
        public EnsureStartupService(ISaveFileService saveFileService, IMessageService messageService, INavigationService navigationService)
        {
            Argument.IsNotNull(() => saveFileService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => navigationService);

            _saveFileService = saveFileService;
            _messageService = messageService;
            _navigationService = navigationService;

            var checkFile = GetCheckFileName();
            SuccessfullyStarted = !File.Exists(checkFile);

            // Always create the file
            Log.Debug("Creating fail safe file check");

            var applicationDataDirectory = GetApplicationDataDirectory();
            if (!Directory.Exists(applicationDataDirectory))
            {
                Directory.CreateDirectory(applicationDataDirectory);
            }

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
            var applicationDataDirectory = GetApplicationDataDirectory();

            Log.Debug("Asking user whether he/she wants to create a backup of the user data");

            switch (await _messageService.Show("It seems that the application failed to start correctly the last time it was started. In order to fix this problem, the software will remove the user data settings. It is recommended that you create a backup. Would you like to create a backup now?", assembly.Title(), MessageButton.YesNoCancel, MessageImage.Warning))
            {
                case MessageResult.Cancel:
                    Log.Info("User choose NOT to delete any data and continue (living on the edge)");
                    return;

                case MessageResult.Yes:
                    Log.Info("User choose to create a backup");

                    if (!await CreateUserDataBackup(applicationDataDirectory))
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

            Log.Debug("Deleting user data from '{0}'", applicationDataDirectory);

            if (Directory.Exists(applicationDataDirectory))
            {
                Directory.Delete(applicationDataDirectory, true);
            }
        }
        #endregion

        #region Methods
        protected virtual string GetApplicationDataDirectory()
        {
            return Catel.IO.Path.GetApplicationDataDirectory();
        }

        private string GetCheckFileName()
        {
            var applicationDataDirectory = GetApplicationDataDirectory();
            var checkFile = Path.Combine(applicationDataDirectory, EnsureStartupCheckFile);

            return checkFile;
        }

        protected virtual async Task<bool> CreateUserDataBackup(string applicationDataDirectory)
        {
            var assembly = AssemblyHelper.GetEntryAssembly();

            _saveFileService.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _saveFileService.FileName = string.Format("{0} backup {1}.zip", assembly.Title(), DateTime.Now.ToString("yyyyMMdd hhmmss"));
            _saveFileService.Filter = "Zip files|*.zip";

            if (!_saveFileService.DetermineFile())
            {
                return false;
            }

            var zipFileName = _saveFileService.FileName;

            Log.Debug("Writing zip file to '{0}'", zipFileName);

            using (var zipFile = new ZipFile())
            {
                zipFile.AddDirectory(applicationDataDirectory, null);
                zipFile.Save(zipFileName);
            }

            return true;
        }
        #endregion
    }
}