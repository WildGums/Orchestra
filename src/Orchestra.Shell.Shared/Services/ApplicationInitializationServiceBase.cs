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
    using Catel.Threading;
    using MethodTimer;

    public class ApplicationInitializationServiceBase : IApplicationInitializationService
    {
        public virtual async Task InitializeBeforeShowingSplashScreenAsync()
        {
            var serviceLocator = this.GetServiceLocator();
            var themeService = serviceLocator.ResolveType<IThemeService>();

            ThemeHelper.EnsureApplicationThemes(typeof(ApplicationInitializationServiceBase).Assembly, themeService.ShouldCreateStyleForwarders());
            ThemeHelper.EnsureApplicationThemes(GetType().Assembly, themeService.ShouldCreateStyleForwarders());
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
    }
}