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
    using MethodTimer;

    public class ApplicationInitializationServiceBase : IApplicationInitializationService
    {
        public virtual bool ShowSplashScreen => true;

        public virtual bool ShowShell => true;

        public virtual async Task InitializeBeforeShowingSplashScreenAsync()
        {
            InitializeLogging();

            var serviceLocator = this.GetServiceLocator();
            var themeService = serviceLocator.ResolveType<IThemeService>();

            // Note: we only have to create style forwarders once
            ThemeHelper.EnsureApplicationThemes(typeof(ApplicationInitializationServiceBase).Assembly, false);
            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, false);

            if (themeService.ShouldCreateStyleForwarders())
            {
                StyleHelper.CreateStyleForwardersForDefaultStyles();
            }
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
