// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrashWarningViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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

    public class CrashWarningViewModel : ViewModelBase
    {
        #region Fields

        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        private readonly IManageAppDataService _manageAppDataService;
        private readonly Assembly _assembly;
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly ILanguageService _languageService;
        #endregion

        #region Constructors
        public CrashWarningViewModel(IManageAppDataService manageAppDataService, IMessageService messageService, INavigationService navigationService,
            ILanguageService languageService)
        {
            ArgumentNullException.ThrowIfNull(messageService);
            ArgumentNullException.ThrowIfNull(navigationService);
            ArgumentNullException.ThrowIfNull(navigationService);
            ArgumentNullException.ThrowIfNull(manageAppDataService);
            ArgumentNullException.ThrowIfNull(languageService);

            _manageAppDataService = manageAppDataService;
            _messageService = messageService;
            _navigationService = navigationService;
            _languageService = languageService;

            _assembly = Catel.Reflection.AssemblyHelper.GetEntryAssembly();

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

            if (!await _manageAppDataService.BackupUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming))
            {
                Log.Warning("User canceled the backup, exit application");

                await _messageService.ShowErrorAsync(_languageService.GetString("Orchestra_FailedToCreateBackup"), _assembly.Title());

                _navigationService.CloseApplication();

                return;
            }

            await _manageAppDataService.DeleteUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming);

            await _messageService.ShowInformationAsync(_languageService.GetString("Orchestra_BackupCreated"), _assembly.Title());

            await CloseViewModelAsync(false);
        }

        public TaskCommand ResetUserSettings { get; set; }

        private async Task OnResetUserSettingsExecuteAsync()
        {
            Log.Info("User choose NOT to create a backup");

            await _manageAppDataService.DeleteUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming);

            await _messageService.ShowInformationAsync(_languageService.GetString("Orchestra_DeletedUserDataSettings"), _assembly.Title());

            await CloseViewModelAsync(false);
        }

        public TaskCommand Continue { get; set; }

        private async Task OnContinueExecuteAsync()
        {
            Log.Info("User choose NOT to delete any data and continue (living on the edge)");

            await CloseViewModelAsync(false);
        }
        #endregion

        protected override Task<bool> CancelAsync()
        {
            Log.Info("User choose NOT to delete any data and continue (living on the edge)");

            return base.CancelAsync();
        }
    }
}
