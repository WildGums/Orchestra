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
    using Orchestra.Services;
    using Views;

    public class MahAppsService : IMahAppsService
    {
        #region Fields
        private readonly ICommandManager _commandManager;
        private readonly IMessageService _messageService;
        #endregion

        #region Constructors
        public MahAppsService(ICommandManager commandManager, IMessageService messageService)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => messageService);

            _commandManager = commandManager;
            _messageService = messageService;
        }
        #endregion

        #region IMahAppsService Members
        public WindowCommands GetRightWindowCommands()
        {
            var windowCommands = new WindowCommands();

            var refreshButton = WindowCommandHelper.CreateWindowCommandButton("appbar_refresh_counterclockwise_down", "refresh");
            refreshButton.Command = _commandManager.GetCommand("File.Refresh");
            _commandManager.RegisterAction("File.Refresh", () => _messageService.Show("Refresh"));
            windowCommands.Items.Add(refreshButton);

            var saveButton = WindowCommandHelper.CreateWindowCommandButton("appbar_save", "save");
            saveButton.Command = _commandManager.GetCommand("File.Save");
            _commandManager.RegisterAction("File.Save", () => _messageService.Show("Save"));
            windowCommands.Items.Add(saveButton);

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
        #endregion
    }
}