// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Models;

    using Orchestra.Services;
    using Orchestra.ViewModels;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IOpenFileService _openFileService;
        private readonly IRecentlyUsedItemsService _recentlyUsedItemsService;

        public RibbonViewModel(INavigationService navigationService, IUIVisualizerService uiVisualizerService,
            ICommandManager commandManager, IRecentlyUsedItemsService recentlyUsedItemsService, IOpenFileService openFileService)
        {
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => recentlyUsedItemsService);
            Argument.IsNotNull(() => openFileService);

            _navigationService = navigationService;
            _uiVisualizerService = uiVisualizerService;
            _recentlyUsedItemsService = recentlyUsedItemsService;
            _openFileService = openFileService;

            Help = new Command(OnHelpExecute);
            Open = new Command(this.OnOpenExecute);
            Exit = new Command(OnExitExecute);
            ShowKeyboardMappings = new Command(OnShowKeyboardMappingsExecute);

            RecentlyUsedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.Items);
            PinnedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.PinnedItems);

            commandManager.RegisterCommand("Help.About", Help, this);
            commandManager.RegisterCommand("File.Open", Open, this);
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
            var aboutInfo = new AboutInfo("/Orchestra.Examples.Ribbon.Microsoft;component/Resources/Images/CompanyLogo.png", "http://www.somecompany.com");
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

        public Command Open { get; private set; }

        private void OnOpenExecute()
        {
            if (_openFileService.DetermineFile())
            {
                _recentlyUsedItemsService.AddItem(new RecentlyUsedItem(_openFileService.FileName, DateTime.Now));
            }
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

        #region
        public List<RecentlyUsedItem> RecentlyUsedItems { get; private set; }

        public List<RecentlyUsedItem> PinnedItems { get; private set; }
        #endregion
    }
}