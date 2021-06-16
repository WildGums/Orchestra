// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Windows;
    using System.Windows.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Windows;
    using MahApps.Metro.Controls;
    using MahApps.Metro.IconPacks;
    using Services;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellWindow"/> class.
        /// </summary>
        public ShellWindow()
            : base(DataWindowMode.Custom, setOwnerAndFocus: false)
        {
            var serviceLocator = ServiceLocator.Default;

            InitializeComponent();

            statusBar.Background = Orc.Theming.ThemeManager.Current.GetThemeColorBrush(Orc.Theming.ThemeColorStyle.AccentColor20);

            serviceLocator.RegisterInstance(pleaseWaitProgressBar, "pleaseWaitService");

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

            if (mahAppsService.GetAboutInfo() is not null)
            {
                var aboutWindowCommand = WindowCommandHelper.CreateWindowCommandButton(new PackIconMaterial { Kind = PackIconMaterialKind.Information }, "About");

                var aboutService = serviceLocator.ResolveType<IAboutService>();
#pragma warning disable AvoidAsyncVoid // Avoid async void
                commandManager.RegisterAction("Help.About", async () => await aboutService.ShowAboutAsync());
#pragma warning restore AvoidAsyncVoid // Avoid async void
                aboutWindowCommand.SetCurrentValue(System.Windows.Controls.Primitives.ButtonBase.CommandProperty, commandManager.GetCommand("Help.About"));

                windowCommands.Items.Add(aboutWindowCommand);
            }

            RightWindowCommands = windowCommands;

            var statusBarContent = mahAppsService.GetStatusBar();
            if (statusBarContent is not null)
            {
                customStatusBarItem.SetCurrentValue(ContentProperty, statusBarContent);
            }

            var mainView = mahAppsService.GetMainView();
            contentControl.Content = mainView;

            ShellDimensionsHelper.ApplyDimensions(this, mainView);

            SetBinding(TitleProperty, new Binding("ViewModel.Title")
            {
                Source = mainView
            });
        }
        #endregion
    }
}
