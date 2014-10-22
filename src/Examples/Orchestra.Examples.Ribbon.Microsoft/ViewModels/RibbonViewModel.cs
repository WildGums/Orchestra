// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orchestra.ViewModels;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUIVisualizerService _uiVisualizerService;

        public RibbonViewModel(INavigationService navigationService, IUIVisualizerService uiVisualizerService,
            ICommandManager commandManager)
        {
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => commandManager);

            _navigationService = navigationService;
            _uiVisualizerService = uiVisualizerService;

            Help = new Command(OnHelpExecute);
            Exit = new Command(OnExitExecute);
            ShowKeyboardMappings = new Command(OnShowKeyboardMappingsExecute);

            commandManager.RegisterCommand("Help.About", Help, this);
            commandManager.RegisterCommand("File.Exit", Exit, this);
        }

        #region Commands
        /// <summary>
        /// Gets the Help command.
        /// </summary>
        public Command Help { get; private set; }

        /// <summary>
        /// Method to invoke when the Help command is executed.
        /// </summary>
        private async void OnHelpExecute()
        {
            var aboutInfo = new AboutInfo("/StockLevels;component/Resources/Images/SMSLogo.png", "http://www.simulationmodelling.com.au");
            await _uiVisualizerService.ShowDialog<AboutViewModel>(aboutInfo);
        }

        /// <summary>
        /// Gets the Exit command.
        /// </summary>
        public Command Exit { get; private set; }

        /// <summary>
        /// Method to invoke when the Exit command is executed.
        /// </summary>
        private void OnExitExecute()
        {
            _navigationService.CloseApplication();
        }

        /// <summary>
        /// Gets the ShowKeyboardMappings command.
        /// </summary>
        public Command ShowKeyboardMappings { get; private set; }

        /// <summary>
        /// Method to invoke when the ShowKeyboardMappings command is executed.
        /// </summary>
        private async void OnShowKeyboardMappingsExecute()
        {
            await _uiVisualizerService.ShowDialog<KeyboardMappingsCustomizationViewModel>();
        }
        #endregion
    }
}