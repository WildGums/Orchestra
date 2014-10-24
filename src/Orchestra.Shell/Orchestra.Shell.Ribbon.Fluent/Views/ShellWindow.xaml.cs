// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellWindow.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using System.Net.Mime;
    using System.Windows;
    using System.Windows.Media;
    using Catel.IoC;
    using Fluent;
    using Services;
    using Shell.Services;

    /// <summary>
    /// Interaction logic for ShellWindow.xaml.
    /// </summary>
    public partial class ShellWindow : IShell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellWindow"/> class.
        /// </summary>
        public ShellWindow()
        {
            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, true);

            // Required for Fluent accent color
            var accentColor = ThemeHelper.GetAccentColor();
            var applicationResources = Application.Current.Resources;
            applicationResources.Add(MetroColors.ThemeColorKey, accentColor);

            InitializeComponent();

            var serviceLocator = ServiceLocator.Default;
            var statusService = serviceLocator.ResolveType<IStatusService>();
            statusService.Initialize(statusTextBlock);

            var dependencyResolver = this.GetDependencyResolver();
            var ribbonService = dependencyResolver.Resolve<IRibbonService>();

            var ribbonContent = ribbonService.GetRibbon();
            if (ribbonContent != null)
            {
                ribbonContentPresenter.Content = ribbonContent;
            }

            var statusBarContent = ribbonService.GetStatusBar();
            if (statusBarContent != null)
            {
                customStatusBarItem.Content = statusBarContent;
            }

            contentPresenter.Content = ribbonService.GetMainView();
        }
    }
}