// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInitializationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Threading;
    using Orc.Theming;
    using Orchestra.Theming;

    public class ApplicationInitializationServiceBase : IApplicationInitializationService
    {
        public virtual bool ShowSplashScreen => true;

        public virtual bool ShowShell => true;

        public virtual async Task InitializeBeforeShowingSplashScreenAsync()
        {
            InitializeLogging();

            var serviceLocator = this.GetServiceLocator();
            var xamlResourceService = serviceLocator.ResolveType<IXamlResourceService>();
            var themeService = serviceLocator.ResolveType<IThemeService>();
            var orchestraThemeManager = serviceLocator.ResolveType<IThemeManager>();
            var orcThemingThemeManager = serviceLocator.ResolveType<Orc.Theming.ThemeManager>();

            // Note: we only have to create style forwarders once
            var xamlResourceDictionaries = xamlResourceService.GetApplicationResourceDictionaries();

            foreach (var xamlResourceDictionary in xamlResourceDictionaries)
            {
                orchestraThemeManager.EnsureApplicationThemes(xamlResourceDictionary, false);
            }

            if (themeService.ShouldCreateStyleForwarders())
            {
                StyleHelper.CreateStyleForwardersForDefaultStyles();
            }

            orcThemingThemeManager.SynchronizeTheme();
        }

        public virtual async Task InitializeBeforeCreatingShellAsync()
        {
        }

        public virtual async Task InitializeAfterCreatingShellAsync()
        {
        }

        public virtual async Task InitializeBeforeShowingShellAsync()
        {
        }

        public virtual async Task InitializeAfterShowingShellAsync()
        {
        }

        protected static async Task RunAndWaitAsync(params Func<Task>[] actions)
        {
            await TaskHelper.RunAndWaitAsync(actions);
        }

        protected virtual void InitializeLogging()
        {
            LogHelper.CleanUpAllLogTypeFiles();

            var fileLogListener = LogHelper.CreateFileLogListener(LogFilePrefixes.EntryAssemblyName);

            fileLogListener.IsDebugEnabled = false;
            fileLogListener.IsInfoEnabled = true;
            fileLogListener.IsWarningEnabled = true;
            fileLogListener.IsErrorEnabled = true;

            LogManager.AddListener(fileLogListener);
        }
    }
}
