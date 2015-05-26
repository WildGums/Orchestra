// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrashWarningWindow.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Views
{
    using Catel.IoC;
    using System.Windows;
    using Services;

    public partial class CrashWarningWindow
    {
        #region Constructors
        public CrashWarningWindow()
        {
            var serviceLocator = ServiceLocator.Default;
            var themeService = serviceLocator.ResolveType<IThemeService>();
            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, themeService.ShouldCreateStyleForwarders());

            InitializeComponent();
        }
        #endregion
    }
}