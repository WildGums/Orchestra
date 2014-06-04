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
    using FallDownMatrixManager.Services;
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
            InitializeComponent();

            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, true);

            var accentColorBrush = ThemeHelper.GetAccentColorBrush();
            MahAppsHelper.SetThemeColor(accentColorBrush.Color);

            border.BorderBrush = accentColorBrush;

            var serviceLocator = ServiceLocator.Default;
            var statusService = serviceLocator.ResolveType<IStatusService>();
            statusService.Initialize(statusTextBlock);

            var dependencyResolver = this.GetDependencyResolver();
            var commandManager = dependencyResolver.Resolve<ICommandManager>();
            var flyoutService = dependencyResolver.Resolve<IFlyoutService>();
            var mahAppsService = dependencyResolver.Resolve<IMahAppsService>();

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

                var aboutService = dependencyResolver.Resolve<IAboutService>();
                commandManager.RegisterAction("Help.About", aboutService.ShowAbout);
                aboutWindowCommand.Command = commandManager.GetCommand("Help.About");

                windowCommands.Items.Add(aboutWindowCommand);
            }

            RightWindowCommands = windowCommands;

            var mainView = mahAppsService.GetMainView();
            contentPresenter.Content = mainView;

            SetBinding(TitleProperty, new Binding("ViewModel.Title") { Source = mainView });
        }
        #endregion
    }
}