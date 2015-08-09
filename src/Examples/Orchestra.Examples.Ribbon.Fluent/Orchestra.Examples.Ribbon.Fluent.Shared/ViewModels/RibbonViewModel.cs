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
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Orchestra.Services;
    using Orchestra.ViewModels;

    public class RibbonViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IRecentlyUsedItemsService _recentlyUsedItemsService;
        private readonly IProcessService _processService;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;

        public RibbonViewModel(INavigationService navigationService, IUIVisualizerService uiVisualizerService,
            ICommandManager commandManager, IRecentlyUsedItemsService recentlyUsedItemsService, IProcessService processService,
            IMessageService messageService, ISelectDirectoryService selectDirectoryService)
        {
            Argument.IsNotNull(() => navigationService);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => recentlyUsedItemsService);
            Argument.IsNotNull(() => processService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => selectDirectoryService);

            _navigationService = navigationService;
            _uiVisualizerService = uiVisualizerService;
            _recentlyUsedItemsService = recentlyUsedItemsService;
            _processService = processService;
            _messageService = messageService;
            _selectDirectoryService = selectDirectoryService;

            OpenProject = new Command(OnOpenProjectExecute);
            OpenRecentlyUsedItem = new Command<string>(OnOpenRecentlyUsedItemExecute);
            OpenInExplorer = new Command<string>(OnOpenInExplorerExecute);
            UnpinItem = new Command<string>(OnUnpinItemExecute);
            PinItem = new Command<string>(OnPinItemExecute);

            ShowKeyboardMappings = new Command(OnShowKeyboardMappingsExecute);

            commandManager.RegisterCommand("File.Open", OpenProject, this);

            var assembly = AssemblyHelper.GetEntryAssembly();
            Title = assembly.Title();
        }

        #region Properties
        public List<RecentlyUsedItem> RecentlyUsedItems { get; private set; }

        public List<RecentlyUsedItem> PinnedItems { get; private set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the OpenProject command.
        /// </summary>
        public Command OpenProject { get; private set; }

        /// <summary>
        /// Method to invoke when the OpenProject command is executed.
        /// </summary>
        private async void OnOpenProjectExecute()
        {
            if (_selectDirectoryService.DetermineDirectory())
            {
                await _messageService.ShowAsync("You have chosen " + _selectDirectoryService.DirectoryName);
            }
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
            await _messageService.ShowAsync(string.Format("Just opened a recently used item: {0}", parameter));
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
            if (!Directory.Exists(parameter))
            {
                await _messageService.ShowWarningAsync("The directory doesn't seem to exist. Cannot open the project in explorer.");
                return;
            }

            _processService.StartProcess(parameter);
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
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            InitializeDemoData();

            _recentlyUsedItemsService.Updated += OnRecentlyUsedItemsServiceUpdated;

            UpdateRecentlyUsedItems();
        }

        protected override Task CloseAsync()
        {
            _recentlyUsedItemsService.Updated -= OnRecentlyUsedItemsServiceUpdated;

            return base.CloseAsync();
        }

        private void OnRecentlyUsedItemsServiceUpdated(object sender, EventArgs e)
        {
            UpdateRecentlyUsedItems();
        }

        private void InitializeDemoData()
        {
            if (_recentlyUsedItemsService.Items.Count() == 0)
            {
                for (var i = 1; i < 4; i++)
                {
                    var item = new RecentlyUsedItem(string.Format("Demo recently used item {0}", i), DateTime.Today.AddDays(i * -1));

                    _recentlyUsedItemsService.AddItem(item);
                }
            }

            if (_recentlyUsedItemsService.PinnedItems.Count() == 0)
            {
                for (var i = 1; i < 4; i++)
                {
                    var item = new RecentlyUsedItem(string.Format("Demo pinned item {0}", i), DateTime.Today.AddDays(i * -1));

                    _recentlyUsedItemsService.AddItem(item);
                    _recentlyUsedItemsService.PinItem(item.Name);
                }
            }
        }

        private void UpdateRecentlyUsedItems()
        {
            RecentlyUsedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.Items);
            PinnedItems = new List<RecentlyUsedItem>(_recentlyUsedItemsService.PinnedItems);
        }
        #endregion
    }
}