namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Markup;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Orc.Theming;
    using Orchestra.Changelog;
    using Orchestra.Changelog.ViewModels;
    using Orchestra.Theming;

    public class ApplicationInitializationServiceBase : IApplicationInitializationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public virtual bool ShowSplashScreen => true;

        public virtual bool ShowShell => true;

        public virtual bool ShowChangelog => true;

        public virtual async Task InitializeBeforeShowingSplashScreenAsync()
        {
            InitializeLogging();

            var xmlLanguage = GetApplicationLanguage();
            InitializeApplicationLanguage(xmlLanguage);

#pragma warning disable IDISP001 // Dispose created.
            var serviceLocator = this.GetServiceLocator();
#pragma warning restore IDISP001 // Dispose created.

            var xamlResourceService = serviceLocator.ResolveRequiredType<IXamlResourceService>();
            var themeService = serviceLocator.ResolveRequiredType<IThemeService>();
            var orchestraThemeManager = serviceLocator.ResolveRequiredType<IThemeManager>();
            var orcThemingThemeManager = serviceLocator.ResolveRequiredType<Orc.Theming.ThemeManager>();

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
            if (ShowChangelog)
            {
                await ShowChangelogAsync();
            }
        }

        protected static async Task RunAndWaitAsync(params Func<Task>[] actions)
        {
            var tasks = new List<Task>();

            foreach (var action in actions)
            {
                tasks.Add(Task.Run(action));
            }

            await Task.WhenAll(tasks);
        }

        protected virtual CultureInfo GetApplicationCulture()
        {
            return CultureInfo.CurrentCulture;
        }

        protected virtual XmlLanguage GetApplicationLanguage()
        {
            var culture = GetApplicationCulture();
            var xmlLanguage = XmlLanguage.GetLanguage(culture.IetfLanguageTag);

            return xmlLanguage;
        }
        
        protected virtual void InitializeApplicationLanguage(XmlLanguage xmlLanguage)
        {
            ArgumentNullException.ThrowIfNull(xmlLanguage);

            Log.Debug($"Setting application language to '{xmlLanguage.IetfLanguageTag}'");

            // Ensure that we are using the right culture
#pragma warning disable WPF0011 // Containing type should be used as registered owner.
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(xmlLanguage));
#pragma warning restore WPF0011 // Containing type should be used as registered owner.
        }

        protected virtual void InitializeLogging()
        {
            LogHelper.CleanUpAllLogTypeFiles();

            var existingFileLogListener = LogManager.GetListeners()
                .FirstOrDefault(x => x is FileLogListener fileLogListener && 
                                     Path.GetFileName(fileLogListener.FilePath).StartsWith(LogFilePrefixes.EntryAssemblyName));
            if (existingFileLogListener is null)
            {
                var fileLogListener = LogHelper.CreateFileLogListener(LogFilePrefixes.EntryAssemblyName);

                fileLogListener.IsDebugEnabled = false;
                fileLogListener.IsInfoEnabled = true;
                fileLogListener.IsWarningEnabled = true;
                fileLogListener.IsErrorEnabled = true;

                LogManager.AddListener(fileLogListener);
            }
        }

        protected virtual async Task ShowChangelogAsync()
        {
            var serviceLocator = ServiceLocator.Default;
            var changelogService = serviceLocator.ResolveRequiredType<IChangelogService>();
            var uiVisualizerService = serviceLocator.ResolveRequiredType<IUIVisualizerService>();

            var changelog = await changelogService.GetChangelogSinceSnapshotAsync();
            if (!changelog.IsEmpty)
            {
                await uiVisualizerService.ShowDialogAsync<ChangelogViewModel>(changelog);
            }
        }
    }
}
