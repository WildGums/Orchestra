// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Services
{
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using global::MahApps.Metro.Controls;
    using Orchestra.Models;
    using Orchestra.Services;
    using ViewModels;
    using Views;

    public class MahAppsService : IMahAppsService
    {
        #region Fields
        private readonly ICommandManager _commandManager;
        private readonly IMessageService _messageService;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public MahAppsService(ICommandManager commandManager, IMessageService messageService, IUIVisualizerService uiVisualizerService)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => uiVisualizerService);

            _commandManager = commandManager;
            _messageService = messageService;
            _uiVisualizerService = uiVisualizerService;
        }
        #endregion

        #region IMahAppsService Members
        public WindowCommands GetRightWindowCommands()
        {
            var windowCommands = new WindowCommands();

            var refreshButton = WindowCommandHelper.CreateWindowCommandButton("appbar_refresh_counterclockwise_down", "refresh");
            refreshButton.Command = _commandManager.GetCommand("File.Refresh");
            _commandManager.RegisterAction("File.Refresh", () => _messageService.ShowAsync("Refresh"));
            windowCommands.Items.Add(refreshButton);

            var saveButton = WindowCommandHelper.CreateWindowCommandButton("appbar_save", "save");
            saveButton.Command = _commandManager.GetCommand("File.Save");
            _commandManager.RegisterAction("File.Save", () => _messageService.ShowAsync("Save"));
            windowCommands.Items.Add(saveButton);

            var showWindowButton = WindowCommandHelper.CreateWindowCommandButton("appbar_new_window", "show window");
            showWindowButton.Command = new Command(() => _uiVisualizerService.ShowDialog<ExampleDialogViewModel>());
            windowCommands.Items.Add(showWindowButton);

            return windowCommands;
        }

        public FlyoutsControl GetFlyouts()
        {
            return null;
        }

        public FrameworkElement GetMainView()
        {
            return new MainView();
        }

        public FrameworkElement GetStatusBar()
        {
            return null;
        }

        public AboutInfo GetAboutInfo()
        {
            return new AboutInfo();
        }
        #endregion
    }
}