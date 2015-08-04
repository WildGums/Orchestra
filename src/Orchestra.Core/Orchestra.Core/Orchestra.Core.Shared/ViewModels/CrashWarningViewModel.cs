// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrashWarningViewModel.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Reflection;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Services;
    using AssemblyHelper = Orchestra.AssemblyHelper;

    public class CrashWarningViewModel : ViewModelBase
    {
        #region Fields

        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        private readonly IAppDataService _appDataService;
        private readonly Assembly _assembly;
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        #endregion

        #region Constructors
        public CrashWarningViewModel(IAppDataService appDataService, IMessageService messageService, INavigationService navigationService)
        {
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => appDataService);

            _appDataService = appDataService;
            _messageService = messageService;
            _navigationService = navigationService;

            _assembly = AssemblyHelper.GetEntryAssembly();

            Continue = new TaskCommand(OnContinueExecuteAsync);
            ResetUserSettings = new TaskCommand(OnResetUserSettingsExecuteAsync);
            BackupAndReset = new TaskCommand(OnResetAndBackupExecuteAsync);
        }
        #endregion

        #region Properties
        public override string Title
        {
            get { return _assembly.Title(); }
        }
        #endregion

        #region Commands
        public TaskCommand BackupAndReset { get; set; }

        private async Task OnResetAndBackupExecuteAsync()
        {
            Log.Info("User choose to create a backup");

            if (!_appDataService.BackupUserData())
            {
                Log.Warning("User canceled the backup, exit application");

                await _messageService.ShowError("Failed to created a backup. To prevent data loss, the application will now exit and not delete any files. Please contact support so they can guide you through the process.", _assembly.Title());

                _navigationService.CloseApplication();

                return;
            }

            _appDataService.DeleteUserData();

            await _messageService.ShowInformation("Backup has been succesfully created.", _assembly.Title());

            await CloseViewModel(false);
        }

        public TaskCommand ResetUserSettings { get; set; }

        private async Task OnResetUserSettingsExecuteAsync()
        {
            Log.Info("User choose NOT to create a backup");

            _appDataService.DeleteUserData();

            await _messageService.ShowInformation("User data settings have been successfully deleted.", _assembly.Title());

            await CloseViewModel(false);
        }

        public TaskCommand Continue { get; set; }

        private async Task OnContinueExecuteAsync()
        {
            Log.Info("User choose NOT to delete any data and continue (living on the edge)");

            await CloseViewModel(false);
        }
        #endregion
    }
}