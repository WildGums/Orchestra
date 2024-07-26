namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using MethodTimer;
    using Orc.Theming;
    using Orchestra.Theming;
    using Views;

    public partial class ShellService : IShellService
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ITypeFactory _typeFactory;
        private readonly ICommandManager _commandManager;
        private readonly IKeyboardMappingsService _keyboardMappingsService;
        private readonly ISplashScreenService _splashScreenService;
        private readonly IEnsureStartupService _ensureStartupService;
        private readonly IApplicationInitializationService _applicationInitializationService;
        private readonly IDependencyResolver _dependencyResolver;
        private readonly IServiceLocator _serviceLocator;
        private readonly IConfigurationBackupService _configurationBackupService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellService" /> class.
        /// </summary>
        /// <param name="typeFactory">The type factory.</param>
        /// <param name="keyboardMappingsService">The keyboard mappings service.</param>
        /// <param name="commandManager">The command manager.</param>
        /// <param name="splashScreenService">The splash screen service.</param>
        /// <param name="ensureStartupService">The ensure startup service.</param>
        /// <param name="applicationInitializationService">The application initialization service.</param>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="configurationBackupService">The configuration backup service</param>
        /// <exception cref="ArgumentNullException">The <paramref name="typeFactory" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="keyboardMappingsService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="commandManager" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="splashScreenService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="applicationInitializationService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="dependencyResolver" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="serviceLocator" /> is <c>null</c>.</exception>
        public ShellService(ITypeFactory typeFactory, IKeyboardMappingsService keyboardMappingsService, ICommandManager commandManager,
            ISplashScreenService splashScreenService, IEnsureStartupService ensureStartupService,
            IApplicationInitializationService applicationInitializationService, IDependencyResolver dependencyResolver,
            IServiceLocator serviceLocator, IConfigurationBackupService configurationBackupService)
        {
            ArgumentNullException.ThrowIfNull(typeFactory);
            ArgumentNullException.ThrowIfNull(keyboardMappingsService);
            ArgumentNullException.ThrowIfNull(commandManager);
            ArgumentNullException.ThrowIfNull(splashScreenService);
            ArgumentNullException.ThrowIfNull(ensureStartupService);
            ArgumentNullException.ThrowIfNull(applicationInitializationService);
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(serviceLocator);
            ArgumentNullException.ThrowIfNull(configurationBackupService);

            _typeFactory = typeFactory;
            _keyboardMappingsService = keyboardMappingsService;
            _commandManager = commandManager;
            _splashScreenService = splashScreenService;
            _ensureStartupService = ensureStartupService;
            _applicationInitializationService = applicationInitializationService;
            _dependencyResolver = dependencyResolver;
            _serviceLocator = serviceLocator;
            _configurationBackupService = configurationBackupService;

            var entryAssembly = Catel.Reflection.AssemblyHelper.GetRequiredEntryAssembly();

            Log.Info("Starting {0} v{1} ({2})", entryAssembly.Title() ?? string.Empty, entryAssembly.Version() ?? string.Empty, entryAssembly.InformationalVersion() ?? string.Empty);

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
                Log.Debug("Showing splash screen");

                var splashScreen = await _splashScreenService.CreateSplashScreenAsync();
                splashScreen.Show();

                shell = await CreateShellInternalAsync<TShell>(splashScreen.Close);
            }
            else
            {
                Log.Debug("Not showing splash screen");

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
        public virtual async Task<TShell> CreateShellInternalAsync<TShell>(Action? postShowShellCallback = null)
            where TShell : IShell
        {
            if (Shell is not null)
            {
                throw Log.ErrorAndCreateException<OrchestraException>("The shell is already created and cannot be created again");
            }

            Log.Info("Checking if software was correctly closed previously");

            await _ensureStartupService.EnsureFailSafeStartupAsync();

            // Maintaining backups, note that we do this in several locations since we want 
            // to ensure a valid backup (and process start time will be used for the file name)
            await _configurationBackupService.BackupAsync();

            TShell? shell = default;
            var successfullyStarted = true;

            try
            {
                var configurationService = _serviceLocator.ResolveRequiredType<IConfigurationService>();
                await configurationService.LoadAsync();

                await InitializeBeforeCreatingShellAsync();

                shell = await CreateShellAsync<TShell>();

                await _keyboardMappingsService.LoadAsync();

                // Now we have a new window, resubscribe the command manager
                _commandManager.SubscribeToKeyboardEvents();

                await InitializeAfterCreatingShellAsync();

                Log.Info("Confirming that application was started successfully");

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
                Log.Error(ex, "An unexpected error occurred, shutting down the application");

                successfullyStarted = false;
            }

            if (!successfullyStarted)
            {
                var entryAssembly = Catel.Reflection.AssemblyHelper.GetRequiredEntryAssembly();
                var assemblyTitle = entryAssembly.Title();

                // Late resolve so user might change the message service
                var messageService = _dependencyResolver.ResolveRequired<IMessageService>();
                await messageService.ShowErrorAsync(string.Format("An unexpected error occurred while starting {0}. Unfortunately it needs to be closed.\n\nPlease try restarting the application. If this error keeps coming up while starting the application, please contact support.", assemblyTitle), string.Format("Failed to start {0}", assemblyTitle));

                Application.Current.Shutdown(-1);
            }

            if (shell is null)
            {
                throw Log.ErrorAndCreateException<OrchestraException>("Failed to create shell, cannot start application");
            }

            return shell;
        }

        [Time]
        private async Task InitializeBeforeCreatingShellAsync()
        {
            Log.Debug("Calling IApplicationInitializationService.InitializeBeforeCreatingShell");

            await _applicationInitializationService.InitializeBeforeCreatingShellAsync();
        }

        [Time]
        private async Task InitializeAfterCreatingShellAsync()
        {
            Log.Debug("Calling IApplicationInitializationService.InitializeAfterCreatingShell");

            await _applicationInitializationService.InitializeAfterCreatingShellAsync();
        }

        partial void OnCreatingShell();

        [Time]
        private async Task<TShell> CreateShellAsync<TShell>()
            where TShell : IShell
        {
            Log.Debug("Creating shell using type '{0}'", typeof(TShell).GetSafeFullName(false));

            // Late resolve so user might change the message service
            var themeService = _dependencyResolver.ResolveRequired<IThemeService>();
            var themeInfo = themeService.GetThemeInfo();

            var shellThemeTypes = TypeCache.GetTypesImplementingInterface(typeof(IShellTheme));

            foreach (var shellThemeType in shellThemeTypes)
            {
                Log.Debug($"Creating shell theme using '{shellThemeType.FullName}'");

                var instance = (IShellTheme)_typeFactory.CreateRequiredInstance(shellThemeType);

                // Register so it stays alive and can subscribe to events
                _serviceLocator.RegisterInstance(instance);

                instance.ApplyTheme(themeInfo);
            }

            OnCreatingShell();

            var shell = _typeFactory.CreateRequiredInstance<TShell>();
            Shell = shell;

            var shellAsWindow = Shell as Window;
            if (shellAsWindow is not null)
            {
                Log.Debug("Setting the new shell as Application.MainWindow");

                shellAsWindow.Owner = null;

                var currentApp = Application.Current;
                currentApp.MainWindow = shellAsWindow;
            }

            OnCreatedShell();

            return shell;
        }

        partial void OnCreatedShell();

        [Time]
        private void ShowShell(IShell shell)
        {
            if (!_applicationInitializationService.ShowShell)
            {
                Log.Debug("Not showing shell");
                return;
            }

            Log.Debug("Showing shell");

            shell.Show();
        }

        [Time]
        private async Task InitializeBeforeShowingShellAsync()
        {
            Log.Debug("Calling IApplicationInitializationService.InitializeBeforeShowingShell");

            await _applicationInitializationService.InitializeBeforeShowingShellAsync();
        }

        [Time]
        private async Task InitializeAfterShowingShellAsync()
        {
            Log.Debug("Calling IApplicationInitializationService.InitializeAfterShowingShell");

            await _applicationInitializationService.InitializeAfterShowingShellAsync();
        }
    }
}
