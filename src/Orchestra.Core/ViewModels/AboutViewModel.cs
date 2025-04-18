namespace Orchestra.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Orchestra.Changelog;
    using Orchestra.Changelog.ViewModels;

    public class AboutViewModel : ViewModelBase
    {
        private readonly IProcessService _processService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        private readonly ILanguageService _languageService;
        private readonly IChangelogService _changelogService;

        public AboutViewModel(AboutInfo aboutInfo, IProcessService processService, IUIVisualizerService uiVisualizerService,
            IMessageService messageService, ILanguageService languageService, IChangelogService changelogService)
        {
            ArgumentNullException.ThrowIfNull(aboutInfo);
            ArgumentNullException.ThrowIfNull(processService);
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(messageService);
            ArgumentNullException.ThrowIfNull(languageService);
            ArgumentNullException.ThrowIfNull(changelogService);

            _processService = processService;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            _languageService = languageService;
            _changelogService = changelogService;

            ValidateUsingDataAnnotations = false;

            var buildDateTime = aboutInfo.BuildDateTime;

            Title = aboutInfo.Name ?? string.Empty;
            Version = string.Format("v {0}", aboutInfo.DisplayVersion);
            BuildDateTime = string.Format(languageService.GetRequiredString("Orchestra_BuiltOn"), buildDateTime?.ToString() ?? "Unknown");
            UriInfo = aboutInfo.UriInfo;
            Copyright = aboutInfo.Copyright;
            CopyrightUrl = aboutInfo.CopyrightUri is null ? null : aboutInfo.CopyrightUri.ToString();
            CompanyLogoUri = aboutInfo.CompanyLogoUri;
            ImageSourceUrl = aboutInfo.LogoImageSource;
            ShowLogButton = aboutInfo.ShowLogButton;
            AppIcon = aboutInfo.AppIcon;

            OpenUrl = new Command(OnOpenUrlExecute, OnOpenUrlCanExecute);
            OpenCopyrightUrl = new Command(OnOpenCopyrightUrlExecute, OnOpenCopyrightUrlCanExecute);
            ShowThirdPartyNotices = new TaskCommand(OnShowThirdPartyNoticesExecuteAsync);
            OpenLog = new TaskCommand(OnOpenLogExecuteAsync);
            ShowChangelog = new TaskCommand(OnShowChangelogExecuteAsync);
            ShowSystemInfo = new TaskCommand(OnShowSystemInfoExecuteAsync);
            EnableDetailedLogging = new Command(OnEnableDetailedLoggingExecute);
        }

        public override string Title { get; protected set; }

        public string Version { get; private set; }

        public string BuildDateTime { get; private set; }

        public UriInfo? UriInfo { get; private set; }

        public string? Copyright { get; private set; }

        public string? CopyrightUrl { get; private set; }

        public Uri? CompanyLogoUri { get; private set; }

        public string? ImageSourceUrl { get; private set; }

        public bool ShowLogButton { get; private set; }

        public bool IsDebugLoggingEnabled { get; private set; }

        public BitmapSource? AppIcon { get; private set; }

        public Command OpenUrl { get; private set; }

        private bool OnOpenUrlCanExecute()
        {
            var uriInfo = UriInfo;
            if (uriInfo is null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(uriInfo.Uri);
        }

        private void OnOpenUrlExecute()
        {
            var uriInfo = UriInfo;
            if (uriInfo is null)
            {
                return;
            }

            _processService.StartProcess(new ProcessContext
            {
                UseShellExecute = true,
                FileName = uriInfo.Uri
            });
        }

        public Command OpenCopyrightUrl { get; private set; }

        private bool OnOpenCopyrightUrlCanExecute()
        {
            return !string.IsNullOrEmpty(CopyrightUrl);
        }

        private void OnOpenCopyrightUrlExecute()
        {
            var copyrightUrl = CopyrightUrl;
            if (string.IsNullOrEmpty(copyrightUrl))
            {
                return;
            }

            _processService.StartProcess(new ProcessContext
            {
                UseShellExecute = true,
                FileName = copyrightUrl
            });
        }

        public TaskCommand ShowThirdPartyNotices { get; private set; }

        private async Task OnShowThirdPartyNoticesExecuteAsync()
        {
            await _uiVisualizerService.ShowDialogAsync<ThirdPartyNoticesViewModel>();
        }

        public TaskCommand OpenLog { get; private set; }

        private async Task OnOpenLogExecuteAsync()
        {
            var fileLogListener = (from logListener in LogManager.GetListeners()
                                   where logListener is FileLogListener
                                   select logListener).FirstOrDefault();
            if (fileLogListener is not null)
            {
                var filePath = ((FileLogListener)fileLogListener).FilePath;

                _processService.StartProcess(new ProcessContext
                {
                    UseShellExecute = true,
                    FileName = filePath
                });
            }
            else
            {
                await _messageService.ShowErrorAsync(_languageService.GetRequiredString("Orchestra_NoLogListenerAvailable"));
            }
        }

        public TaskCommand ShowChangelog { get; private set; }

        private async Task OnShowChangelogExecuteAsync()
        {
            var changelog = await _changelogService.GetChangelogAsync();

            await _uiVisualizerService.ShowDialogAsync<ChangelogViewModel>(changelog);
        }

        public TaskCommand ShowSystemInfo { get; private set; }

        private Task OnShowSystemInfoExecuteAsync()
        {
            return _uiVisualizerService.ShowDialogAsync<SystemInfoViewModel>();
        }

        public Command EnableDetailedLogging { get; private set; }

        private void OnEnableDetailedLoggingExecute()
        {
            LogManager.IsDebugEnabled = true;

            foreach (var logListener in LogManager.GetListeners())
            {
                logListener.IsDebugEnabled = true;
            }

            UpdateLoggingInfo();
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            UpdateLoggingInfo();
        }

        private void UpdateLoggingInfo()
        {
            var isDebugLoggingEnabled = true;

            foreach (var logListener in LogManager.GetListeners())
            {
                if (!logListener.IsDebugEnabled)
                {
                    isDebugLoggingEnabled = false;
                    break;
                }
            }

            IsDebugLoggingEnabled = isDebugLoggingEnabled;
        }
    }
}
