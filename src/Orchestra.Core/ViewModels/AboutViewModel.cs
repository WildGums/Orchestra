// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Reflection;
    using Models;
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
            Argument.IsNotNull(() => aboutInfo);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => languageService);
            Argument.IsNotNull(() => changelogService);

            _processService = processService;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            _languageService = languageService;
            _changelogService = changelogService;

            var buildDateTime = aboutInfo.BuildDateTime.Value;

            Title = aboutInfo.Name;
            Version = string.Format("v {0}", aboutInfo.DisplayVersion);
            BuildDateTime = string.Format(languageService.GetString("Orchestra_BuiltOn"), buildDateTime);
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

        #region Properties
        public override string Title { get; protected set; }

        public string Version { get; private set; }

        public string BuildDateTime { get; private set; }

        public UriInfo UriInfo { get; private set; }

        public string Copyright { get; private set; }

        public string CopyrightUrl { get; private set; }

        public Uri CompanyLogoUri { get; private set; }

        public string ImageSourceUrl { get; private set; }

        public bool ShowLogButton { get; private set; }

        public bool IsDebugLoggingEnabled { get; private set; }

        public BitmapSource AppIcon { get; private set; }
        #endregion

        #region Commands
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
            _processService.StartProcess(new ProcessContext
            {
                UseShellExecute = true,
                FileName = UriInfo.Uri
            });
        }

        public Command OpenCopyrightUrl { get; private set; }

        private bool OnOpenCopyrightUrlCanExecute()
        {
            return !string.IsNullOrEmpty(CopyrightUrl);
        }

        private void OnOpenCopyrightUrlExecute()
        {
            _processService.StartProcess(new ProcessContext
            {
                UseShellExecute = true,
                FileName = CopyrightUrl
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
                await _messageService.ShowErrorAsync(_languageService.GetString("Orchestra_NoLogListenerAvailable"));
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
        #endregion

        #region Methods
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
        #endregion
    }
}
