// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using Catel.IoC;
    using Catel.Logging;
    using Markup;
    using Orchestra.Services;
    using Orchestra.Views;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly DateTime _start;
        private readonly Stopwatch _stopwatch;
        private DateTime _end;
        #endregion

        #region Constructors
        public App()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            _start = DateTime.Now;
        }
        #endregion

        #region Methods
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            FontImage.RegisterFont("FontAwesome", new FontFamily(new Uri("pack://application:,,,/Orchestra.Examples.Ribbon;component/Resources/Fonts/", UriKind.RelativeOrAbsolute), "./#FontAwesome"));

            FontImage.DefaultFontFamily = "FontAwesome";

            var serviceLocator = ServiceLocator.Default;
            var shellService = serviceLocator.ResolveType<IShellService>();
            shellService.CreateWithSplash<ShellWindow>();

            _end = DateTime.Now;
            _stopwatch.Stop();

            Log.Info("Elapsed startup stopwatch time: {0}", _stopwatch.Elapsed);
            Log.Info("Elapsed startup time: {0}", _end - _start);

            var aboutInfoService = serviceLocator.ResolveType<IAboutInfoService>();
            var aboutInfo = aboutInfoService.GetAboutInfo();
            var assembly = aboutInfo.Assembly;
            var version = VersionHelper.GetCurrentVersion(assembly);

            Log.Info("Version: {0}", version);
        }
        #endregion
    }
}