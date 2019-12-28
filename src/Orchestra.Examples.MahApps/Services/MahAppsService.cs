// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Services
{
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using global::MahApps.Metro.Controls;
    using global::MahApps.Metro.IconPacks;
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

            var refreshButton = WindowCommandHelper.CreateWindowCommandButton(new PackIconMaterial { Kind = PackIconMaterialKind.Refresh }, "Refresh");
            refreshButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, _commandManager.GetCommand("File.Refresh"));
            _commandManager.RegisterAction("File.Refresh", () => _messageService.ShowAsync("Refresh"));
            windowCommands.Items.Add(refreshButton);

            var saveButton = WindowCommandHelper.CreateWindowCommandButton(new PackIconMaterial { Kind = PackIconMaterialKind.ContentSave }, "Save");
            saveButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, _commandManager.GetCommand("File.Save"));
            _commandManager.RegisterAction("File.Save", () => _messageService.ShowAsync("Save"));
            windowCommands.Items.Add(saveButton);

            var showWindowButton = WindowCommandHelper.CreateWindowCommandButton(new PackIconMaterial { Kind = PackIconMaterialKind.OpenInNew }, "Show dialog window");
            showWindowButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, new TaskCommand(() => _uiVisualizerService.ShowDialogAsync<ExampleDialogViewModel>()));
            windowCommands.Items.Add(showWindowButton);

            var showDataWindowButton = WindowCommandHelper.CreateWindowCommandButton(new PackIconMaterial { Kind = PackIconMaterialKind.OpenInNew }, "Show data window");
            showDataWindowButton.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, new TaskCommand(() => _uiVisualizerService.ShowDialogAsync<ExampleDataViewModel>()));
            windowCommands.Items.Add(showDataWindowButton);
            
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
