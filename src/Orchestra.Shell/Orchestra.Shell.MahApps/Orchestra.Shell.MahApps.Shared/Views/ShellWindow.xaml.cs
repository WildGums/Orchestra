// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Windows;
    using System.Windows.Data;
    using Windows;
    using Catel.IoC;
    using Catel.MVVM;
    using MahApps.Metro.Controls;
    using Services;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ShellWindow : MetroDataWindow, IShell
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellWindow"/> class.
        /// </summary>
        public ShellWindow()
        {
            var serviceLocator = ServiceLocator.Default;

            var themeService = serviceLocator.ResolveType<IThemeService>();
            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, themeService.ShouldCreateStyleForwarders());

            MahAppsHelper.ApplyTheme();

            InitializeComponent();

            serviceLocator.RegisterInstance(pleaseWaitProgressBar, "pleaseWaitService");

            var accentColorBrush = ThemeHelper.GetAccentColorBrush();
            border.BorderBrush = accentColorBrush;

            var statusService = serviceLocator.ResolveType<IStatusService>();
            statusService.Initialize(statusTextBlock);

            var commandManager = serviceLocator.ResolveType<ICommandManager>();
            var flyoutService = serviceLocator.ResolveType<IFlyoutService>();
            var mahAppsService = serviceLocator.ResolveType<IMahAppsService>();

            serviceLocator.RegisterInstance<IAboutInfoService>(mahAppsService);

            var flyouts = new FlyoutsControl();
            foreach (var flyout in flyoutService.GetFlyouts())
            {
                flyouts.Items.Add(flyout);
            }

            Flyouts = flyouts;

            var windowCommands = mahAppsService.GetRightWindowCommands();

            if (mahAppsService.GetAboutInfo() != null)
            {
                var aboutWindowCommand = WindowCommandHelper.CreateWindowCommandButton("appbar_information", "about");

                var aboutService = serviceLocator.ResolveType<IAboutService>();
                commandManager.RegisterAction("Help.About", aboutService.ShowAbout);
                aboutWindowCommand.Command = commandManager.GetCommand("Help.About");

                windowCommands.Items.Add(aboutWindowCommand);
            }

            RightWindowCommands = windowCommands;

            var statusBarContent = mahAppsService.GetStatusBar();
            if (statusBarContent != null)
            {
                customStatusBarItem.Content = statusBarContent;
            }

            var mainView = mahAppsService.GetMainView();
            contentControl.Content = mainView;

            SetBinding(TitleProperty, new Binding("ViewModel.Title") { Source = mainView });
        }
        #endregion
    }
}