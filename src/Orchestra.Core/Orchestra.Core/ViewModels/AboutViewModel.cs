// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Reflection;
    using Orchestra;
    using Models;

    public class AboutViewModel : ViewModelBase
    {
        private readonly IProcessService _processService;

        public AboutViewModel(AboutInfo aboutInfo, IProcessService processService)
        {
            Argument.IsNotNull(() => aboutInfo);
            Argument.IsNotNull(() => processService);

            _processService = processService;

            var assembly = Orchestra.AssemblyHelper.GetEntryAssembly();

            Title = assembly.Title();
            Version = string.Format("v {0}", assembly.Version());
            Url = aboutInfo.Url;
            Copyright = assembly.Copyright();
            ImageSourceUrl = aboutInfo.LogoImageSource;

            OpenUrl = new Command(OnOpenUrlExecute);
            EnableLogging = new Command(OnEnableLoggingExecute, OnEnableLoggingCanExecute);
        }

        #region Properties
        public override string Title { get; protected set; }

        public string Version { get; private set; }

        public string Url { get; private set; }

        public string Copyright { get; private set; }

        public string ImageSourceUrl { get; private set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the OpenUrl command.
        /// </summary>
        public Command OpenUrl { get; private set; }

        /// <summary>
        /// Method to invoke when the OpenUrl command is executed.
        /// </summary>
        private void OnOpenUrlExecute()
        {
            _processService.StartProcess(Url);
        }

        /// <summary>
        /// Gets the EnableLogging command.
        /// </summary>
        public Command EnableLogging { get; private set; }

        /// <summary>
        /// Method to check whether the EnableLogging command can be executed.
        /// </summary>
        private bool OnEnableLoggingCanExecute()
        {
            return !LogManager.GetListeners().Any(x => x is FileLogListener);
        }

        /// <summary>
        /// Method to invoke when the EnableLogging command is executed.
        /// </summary>
        private void OnEnableLoggingExecute()
        {
            LogHelper.AddFileLogListener();
        }
        #endregion
    }
}