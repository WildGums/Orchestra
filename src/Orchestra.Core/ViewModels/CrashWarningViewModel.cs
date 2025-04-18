namespace Orchestra.Views
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Services;

    public class CrashWarningViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        
        private readonly IManageAppDataService _manageAppDataService;
        private readonly Assembly _assembly;
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly ILanguageService _languageService;

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

            ValidateUsingDataAnnotations = false;

            _assembly = Catel.Reflection.AssemblyHelper.GetRequiredEntryAssembly();

            Continue = new TaskCommand(OnContinueExecuteAsync);
            ResetUserSettings = new TaskCommand(OnResetUserSettingsExecuteAsync);
            BackupAndReset = new TaskCommand(OnResetAndBackupExecuteAsync);
        }

        public override string Title
        {
            get { return _assembly.Title() ?? string.Empty; }
        }

        public TaskCommand BackupAndReset { get; set; }

        private async Task OnResetAndBackupExecuteAsync()
        {
            Log.Info("User choose to create a backup");

            if (!await _manageAppDataService.BackupUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming))
            {
                Log.Warning("User canceled the backup, exit application");

                await _messageService.ShowErrorAsync(_languageService.GetRequiredString("Orchestra_FailedToCreateBackup"), _assembly.Title() ?? string.Empty);

                await _navigationService.CloseApplicationAsync();

                return;
            }

            await _manageAppDataService.DeleteUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming);

            await _messageService.ShowInformationAsync(_languageService.GetRequiredString("Orchestra_BackupCreated"), _assembly.Title() ?? string.Empty);

            await CloseViewModelAsync(false);
        }

        public TaskCommand ResetUserSettings { get; set; }

        private async Task OnResetUserSettingsExecuteAsync()
        {
            Log.Info("User choose NOT to create a backup");

            await _manageAppDataService.DeleteUserDataAsync(Catel.IO.ApplicationDataTarget.UserRoaming);

            await _messageService.ShowInformationAsync(_languageService.GetRequiredString("Orchestra_DeletedUserDataSettings"), _assembly.Title() ?? string.Empty);

            await CloseViewModelAsync(false);
        }

        public TaskCommand Continue { get; set; }

        private async Task OnContinueExecuteAsync()
        {
            Log.Info("User choose NOT to delete any data and continue (living on the edge)");

            await CloseViewModelAsync(false);
        }

        protected override Task<bool> CancelAsync()
        {
            Log.Info("User choose NOT to delete any data and continue (living on the edge)");

            return base.CancelAsync();
        }
    }
}
