// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using Windows;
    using Catel.IoC;
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

            MahAppsHelper.SetThemeColor(ThemeHelper.GetAccentColor());

            var serviceLocator = ServiceLocator.Default;
            var statusService = serviceLocator.ResolveType<IStatusService>();
            statusService.Initialize(statusTextBlock);

            var dependencyResolver = this.GetDependencyResolver();
            var mahAppsService = dependencyResolver.Resolve<IMahAppsService>();
            var flyoutService = dependencyResolver.Resolve<IFlyoutService>();

            Flyouts = new FlyoutsControl();
            foreach (var flyout in flyoutService.GetFlyouts())
            {
                Flyouts.Items.Add(flyout);
            }

            RightWindowCommands = mahAppsService.GetRightWindowCommands();
            contentPresenter.Content = mahAppsService.GetMainView();
        }
        #endregion
    }
}