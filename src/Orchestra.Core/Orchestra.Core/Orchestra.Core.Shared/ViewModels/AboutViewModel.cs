// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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

    public class AboutViewModel : ViewModelBase
    {
        private readonly IProcessService _processService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        private readonly ILanguageService _languageService;

        public AboutViewModel(AboutInfo aboutInfo, IProcessService processService, IUIVisualizerService uiVisualizerService,
            IMessageService messageService, ILanguageService languageService)
        {
            Argument.IsNotNull(() => aboutInfo);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => languageService);

            _processService = processService;
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            _languageService = languageService;

            var assembly = aboutInfo.Assembly;
            var version = VersionHelper.GetCurrentVersion(assembly);
            var buildDateTime = assembly.GetBuildDateTime();

            Title = assembly.Title();
            Version = string.Format("v {0}", version);
            BuildDateTime = string.Format(languageService.GetString("Orchestra_BuiltOn"), buildDateTime);
            UriInfo = aboutInfo.UriInfo;
            Copyright = assembly.Copyright();
            CompanyLogoUri = aboutInfo.CompanyLogoUri;
            ImageSourceUrl = aboutInfo.LogoImageSource;
            ShowLogButton = aboutInfo.ShowLogButton;
            AppIcon = assembly.ExtractLargestIcon();
            OpenUrl = new Command(OnOpenUrlExecute, OnOpenUrlCanExecute);
            OpenLog = new TaskCommand(OnOpenLogExecuteAsync);
            ShowSystemInfo = new Command(OnShowSystemInfoExecute);
            EnableDetailedLogging = new Command(OnEnableDetailedLoggingExecute);
        }

        #region Properties
        public override string Title { get; protected set; }

        public string Version { get; private set; }

        public string BuildDateTime { get; private set; }

        public UriInfo UriInfo { get; private set; }

        public string Copyright { get; private set; }

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
            if (uriInfo == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(uriInfo.Uri);
        }

        private void OnOpenUrlExecute()
        {
            _processService.StartProcess(UriInfo.Uri);
        }

        public TaskCommand OpenLog { get; private set; }

        private async Task OnOpenLogExecuteAsync()
        {
            var fileLogListener = (from logListener in LogManager.GetListeners()
                                   where logListener is FileLogListener
                                   select logListener).FirstOrDefault();
            if (fileLogListener != null)
            {
                var filePath = ((FileLogListener) fileLogListener).FilePath;

                _processService.StartProcess(filePath);
            }
            else
            {
                await _messageService.ShowErrorAsync(_languageService.GetString("Orchestra_NoLogListenerAvailable"));
            }
        }

        public Command ShowSystemInfo { get; private set; }

        private void OnShowSystemInfoExecute()
        {
            _uiVisualizerService.ShowDialog<SystemInfoViewModel>();
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