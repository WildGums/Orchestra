namespace Orchestra
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using MethodTimer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Orc.Theming;
    using Orchestra.Theming;
    using Views;

    public partial class ShellService : IShellService
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(ShellService));

        private readonly ICommandManager _commandManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IKeyboardMappingsService _keyboardMappingsService;
        private readonly ISplashScreenService _splashScreenService;
        private readonly IEnsureStartupService _ensureStartupService;
        private readonly IApplicationInitializationService _applicationInitializationService;
        private readonly IConfigurationBackupService _configurationBackupService;

        public ShellService(IServiceProvider serviceProvider, IKeyboardMappingsService keyboardMappingsService, ICommandManager commandManager,
            ISplashScreenService splashScreenService, IEnsureStartupService ensureStartupService,
            IApplicationInitializationService applicationInitializationService, IConfigurationBackupService configurationBackupService)
        {
            _serviceProvider = serviceProvider;
            _keyboardMappingsService = keyboardMappingsService;
            _commandManager = commandManager;
            _splashScreenService = splashScreenService;
            _ensureStartupService = ensureStartupService;
            _applicationInitializationService = applicationInitializationService;
            _configurationBackupService = configurationBackupService;

            var entryAssembly = Catel.Reflection.AssemblyHelper.GetRequiredEntryAssembly();

            Logger.LogInformation("Starting {0} v{1} ({2})", entryAssembly.Title() ?? string.Empty, entryAssembly.Version() ?? string.Empty, entryAssembly.InformationalVersion() ?? string.Empty);

            // Initialize (now we have an application)
            DotNetPatchHelper.Initialize();
        }

        /// <summary>
        /// Gets the shell.
        /// </summary>
        /// <value>The shell.</value>
        public IShell? Shell { get; private set; }

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        [Time]
        public virtual async Task<TShell> CreateAsync<TShell>()
            where TShell : class, IShell
        {
            await _applicationInitializationService.InitializeBeforeShowingSplashScreenAsync();

            TShell shell;

            if (_applicationInitializationService.ShowSplashScreen)
            {
                Logger.LogDebug("Showing splash screen");

                var splashScreen = await _splashScreenService.CreateSplashScreenAsync();
                splashScreen.Show();

                shell = await CreateShellInternalAsync<TShell>(splashScreen.Close);
            }
            else
            {
                Logger.LogDebug("Not showing splash screen");

                // Note: it's important to change the application mode. If we are not showing a splash screen,
                // the app won't have a window and will immediately close (if we start any task that is awaited)
                var application = Application.Current;
                var currentApplicationCloseMode = application.ShutdownMode;
                application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

                shell = await CreateShellInternalAsync<TShell>();

                application.ShutdownMode = currentApplicationCloseMode;
            }

            return shell;
        }

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <param name="postShowShellCallback">The shell created callback.</param>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        private async Task<TShell> CreateShellInternalAsync<TShell>(Action? postShowShellCallback = null)
            where TShell : IShell
        {
            if (Shell is not null)
            {
                throw Logger.LogErrorAndCreateException<OrchestraException>("The shell is already created and cannot be created again");
            }

            Logger.LogInformation("Checking if software was correctly closed previously");

            await _ensureStartupService.EnsureFailSafeStartupAsync();

            // Maintaining backups, note that we do this in several locations since we want 
            // to ensure a valid backup (and process start time will be used for the file name)
            await _configurationBackupService.BackupAsync();

            TShell? shell = default;
            Exception? exception = null;
            var successfullyStarted = true;

            try
            {
                var configurationService = _serviceProvider.GetRequiredService<IConfigurationService>();
                await configurationService.LoadAsync();

                await InitializeBeforeCreatingShellAsync();

                shell = await CreateShellAsync<TShell>();

                await _keyboardMappingsService.LoadAsync();

                // Now we have a new window, resubscribe the command manager
                _commandManager.SubscribeToKeyboardEvents();

                await InitializeAfterCreatingShellAsync();

                Logger.LogInformation("Confirming that application was started successfully");

                await _ensureStartupService.ConfirmApplicationStartedSuccessfullyAsync();

                await InitializeBeforeShowingShellAsync();

                ShowShell(shell);

                if (postShowShellCallback is not null)
                {
                    postShowShellCallback();
                }

                await InitializeAfterShowingShellAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An unexpected error occurred, shutting down the application");

                successfullyStarted = false;
                exception = ex;
            }

            if (!successfullyStarted)
            {
                // Allow custom shell recovery process, but end app anyway
                var shellRecoveryService = _serviceProvider.GetRequiredService<IShellRecoveryService>();

                await shellRecoveryService.StartRecoveryAsync(new ShellRecoveryContext
                {
                    Exception = exception,
                    Shell = shell
                });

                Application.Current.Shutdown(-1);
                return default!;
            }

            if (shell is null)
            {
                throw Logger.LogErrorAndCreateException<OrchestraException>("Failed to create shell, cannot start application");
            }

            return shell;
        }

        [Time]
        private async Task InitializeBeforeCreatingShellAsync()
        {
            Logger.LogDebug("Calling IApplicationInitializationService.InitializeBeforeCreatingShell");

            await _applicationInitializationService.InitializeBeforeCreatingShellAsync();
        }

        [Time]
        private async Task InitializeAfterCreatingShellAsync()
        {
            Logger.LogDebug("Calling IApplicationInitializationService.InitializeAfterCreatingShell");

            await _applicationInitializationService.InitializeAfterCreatingShellAsync();
        }

        partial void OnCreatingShell();

        [Time]
        protected virtual async Task<TShell> CreateShellAsync<TShell>()
            where TShell : IShell
        {
            Logger.LogDebug("Creating shell using type '{0}'", typeof(TShell).GetSafeFullName(false));

            // Late resolve so user might change the message service
            var themeService = _serviceProvider.GetRequiredService<IThemeService>();
            var themeInfo = themeService.GetThemeInfo();

            var shellThemeTypes = TypeCache.GetTypesImplementingInterface(typeof(IShellTheme));

            foreach (var shellThemeType in shellThemeTypes)
            {
                Logger.LogDebug($"Creating shell theme using '{shellThemeType.FullName}'");

                var instance = (IShellTheme)ActivatorUtilities.CreateInstance(_serviceProvider, shellThemeType);

                //// Register so it stays alive and can subscribe to events
                //_serviceLocator.RegisterInstance(instance);

                instance.ApplyTheme(themeInfo);
            }

            OnCreatingShell();

            var shell = ActivatorUtilities.CreateInstance<TShell>(_serviceProvider);
            Shell = shell;

            var shellAsWindow = Shell as Window;
            if (shellAsWindow is not null)
            {
                Logger.LogDebug("Setting the new shell as Application.MainWindow");

                shellAsWindow.Owner = null;

                var currentApp = Application.Current;
                currentApp.MainWindow = shellAsWindow;
            }

            OnCreatedShell();

            return shell;
        }

        partial void OnCreatedShell();

        [Time]
        protected virtual void ShowShell(IShell shell)
        {
            if (!_applicationInitializationService.ShowShell)
            {
                Logger.LogDebug("Not showing shell");
                return;
            }

            Logger.LogDebug("Showing shell");

            shell.Show();
        }

        [Time]
        private async Task InitializeBeforeShowingShellAsync()
        {
            Logger.LogDebug("Calling IApplicationInitializationService.InitializeBeforeShowingShell");

            await _applicationInitializationService.InitializeBeforeShowingShellAsync();
        }

        [Time]
        private async Task InitializeAfterShowingShellAsync()
        {
            Logger.LogDebug("Calling IApplicationInitializationService.InitializeAfterShowingShell");

            await _applicationInitializationService.InitializeAfterShowingShellAsync();
        }
    }
}
