// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Drawing;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Reflection;
    using Catel.Windows.Media.Imaging;
    using Models;
    using Services;

    public class AboutViewModel : ViewModelBase
    {
        private readonly IProcessService _processService;
        private readonly IUIVisualizerService _uiVisualizerService;

        public AboutViewModel(AboutInfo aboutInfo, IProcessService processService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => aboutInfo);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => uiVisualizerService);

            _processService = processService;
            _uiVisualizerService = uiVisualizerService;

            var assembly = aboutInfo.Assembly;
            var version = VersionHelper.GetCurrentVersion(assembly);
            var buildDateTime = assembly.GetBuildDateTime();

            Title = assembly.Title();
            Version = string.Format("v {0}", version);
            BuildDateTime = string.Format("Built on {0}", buildDateTime);
            Url = aboutInfo.Url;
            Copyright = assembly.Copyright();
            ImageSourceUrl = aboutInfo.LogoImageSource;
            ShowLogButton = aboutInfo.ShowLogButton;
            AppIcon = assembly.ExtractLargestIcon();
            OpenUrl = new Command(OnOpenUrlExecute);
            OpenLog = new Command(OnOpenLogExecute);
            ShowSystemInfo = new Command(OnShowSystemInfoExecute);
        }

        #region Properties
        public override string Title { get; protected set; }

        public string Version { get; private set; }

        public string BuildDateTime { get; private set; }

        public string Url { get; private set; }

        public string Copyright { get; private set; }

        public string ImageSourceUrl { get; private set; }

        public bool ShowLogButton { get; private set; }

        public BitmapSource AppIcon { get; private set; }
        #endregion

        #region Commands
        public Command OpenUrl { get; private set; }

        private void OnOpenUrlExecute()
        {
            _processService.StartProcess(Url);
        }

        public Command OpenLog { get; private set; }

        private void OnOpenLogExecute()
        {
            var fileLogListener = (from logListener in LogManager.GetListeners()
                                   where logListener is FileLogListener
                                   select logListener).FirstOrDefault();
            if (fileLogListener != null)
            {
                var filePath = ((FileLogListener) fileLogListener).FilePath;

                _processService.StartProcess(filePath);
            }
        }

        public Command ShowSystemInfo { get; private set; }

        private async void OnShowSystemInfoExecute()
        {
            await _uiVisualizerService.ShowDialog<SystemInfoViewModel>();
        }
        #endregion
    }
}