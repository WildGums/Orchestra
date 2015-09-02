// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

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
        private readonly IMessageService _messageService;
        private readonly IProcessService _processService;

        public RibbonViewModel(INavigationService navigationService, 
            IUIVisualizerService uiVisualizerService,
            ICommandManager commandManager, 
            IRecentlyUsedItemsService recentlyUsedItemsService, 
            IOpenFileService openFileService,
            IMessageService messageService,
            IProcessService processService)
        {
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => recentlyUsedItemsService);
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => processService);

            _navigationService = navigationService;
            _uiVisualizerService = uiVisualizerService;
            _recentlyUsedItemsService = recentlyUsedItemsService;
            _openFileService = openFileService;
            _messageService = messageService;
            _processService = processService;

            Help = new Command(OnHelpExecute);
            Open = new Command(this.OnOpenExecute);
            Exit = new Command(OnExitExecute);
            ShowKeyboardMappings = new Command(OnShowKeyboardMappingsExecute);

            OpenRecentlyUsedItem = new Command<string>(OnOpenRecentlyUsedItemExecute);
            UnpinItem = new Command<string>(OnUnpinItemExecute);
            PinItem = new Command<string>(OnPinItemExecute);
            OpenInExplorer = new Command<string>(OnOpenInExplorerExecute);

            OnRecentlyUsedItemsServiceUpdated(null, null);

            commandManager.RegisterCommand("Help.About", Help, this);
            commandManager.RegisterCommand("File.Open", Open, this);
            commandManager.RegisterCommand("File.Exit", Exit, this);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _recentlyUsedItemsService.Updated += OnRecentlyUsedItemsServiceUpdated;
        }

        protected override async Task CloseAsync()
        {
            _recentlyUsedItemsService.Updated -= OnRecentlyUsedItemsServiceUpdated;

            await base.CloseAsync();
        }

        #region Commands
        /// <summary>
        /// Gets the Help command.
        /// </summary>
        public Command Help { get; private set; }

        /// <summary>
        /// Method to invoke when the Help command is executed.
        /// </summary>
        private void OnHelpExecute()
        {
            var aboutInfo = new AboutInfo(new Uri("pack://application:,,,/Resources/Images/CompanyLogo.png", UriKind.RelativeOrAbsolute), 
                "/Orchestra.Examples.Ribbon.Microsoft;component/Resources/Images/CompanyLogo.png", 
                new UriInfo("http://www.catelproject.com", "Product website"));
            _uiVisualizerService.ShowDialog<AboutViewModel>(aboutInfo);
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
        private void OnShowKeyboardMappingsExecute()
        {
            _uiVisualizerService.ShowDialog<KeyboardMappingsCustomizationViewModel>();
        }

        /// <summary>
        /// Gets the OpenRecentlyUsedItem command.
        /// </summary>
        public Command<string> OpenRecentlyUsedItem { get; private set; }

        /// <summary>
        /// Method to invoke when the OpenRecentlyUsedItem command is executed.
        /// </summary>
        private async void OnOpenRecentlyUsedItemExecute(string parameter)
        {
            var failed = false;

            try
            {
                // TODO replace following line with actual load logic
                failed = !File.Exists(parameter);
            }
            catch (Exception)
            {
                failed = true;
            }

            if (failed)
            {
                if (await _messageService.ShowAsync("The file does not exist or has been removed. Would you like to remove it from the recently used list?", "Remove from recently used items?", MessageButton.YesNo) == MessageResult.Yes)
                {
                    var recentlyUsedItem = _recentlyUsedItemsService.Items.FirstOrDefault(x => string.Equals(x.Name, parameter));
                    if (recentlyUsedItem != null)
                    {
                        _recentlyUsedItemsService.RemoveItem(recentlyUsedItem);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the Unpin command.
        /// </summary>
        public Command<string> UnpinItem { get; private set; }

        /// <summary>
        /// Method to invoke when the Unpin command is executed.
        /// </summary>
        private void OnUnpinItemExecute(string parameter)
        {
            _recentlyUsedItemsService.UnpinItem(parameter);
        }

        /// <summary>
        /// Gets the Pin command.
        /// </summary>
        public Command<string> PinItem { get; private set; }

        /// <summary>
        /// Method to invoke when the Pin command is executed.
        /// </summary>
        private void OnPinItemExecute(string parameter)
        {
            _recentlyUsedItemsService.PinItem(parameter);
        }

        /// <summary>
        /// Gets the OpenInExplorer command.
        /// </summary>
        public Command<string> OpenInExplorer { get; private set; }

        /// <summary>
        /// Method to invoke when the OpenInExplorer command is executed.
        /// </summary>
        private async void OnOpenInExplorerExecute(string parameter)
        {
            if (!File.Exists(parameter))
            {
                await _messageService.ShowWarningAsync("The file doesn't seem to exist. Cannot open the project in explorer.");
                return;
            }

            _processService.StartProcess(parameter);
        }
        #endregion

        #region
        public List<RecentlyUsedItem> RecentlyUsedItems { get; private set; }

        public List<RecentlyUsedItem> PinnedItems { get; private set; }
        #endregion

        private void OnRecentlyUsedItemsServiceUpdated(object sender, EventArgs e)
        {
            RecentlyUsedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.Items);
            PinnedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.PinnedItems);
        }
    }
}