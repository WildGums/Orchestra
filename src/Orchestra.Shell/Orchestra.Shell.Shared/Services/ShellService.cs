// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Catel.Threading;
    using MethodTimer;
    using Views;
    using AssemblyHelper = Orchestra.AssemblyHelper;

    public partial class ShellService : IShellService
    {
        #region Fields
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
        #endregion

        #region Constructors
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
        /// <exception cref="ArgumentNullException">The <paramref name="typeFactory" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="keyboardMappingsService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="commandManager" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="splashScreenService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="applicationInitializationService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="dependencyResolver" /> is <c>null</c>.</exception>
        public ShellService(ITypeFactory typeFactory, IKeyboardMappingsService keyboardMappingsService, ICommandManager commandManager,
            ISplashScreenService splashScreenService, IEnsureStartupService ensureStartupService, IApplicationInitializationService applicationInitializationService, IDependencyResolver dependencyResolver)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => keyboardMappingsService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => splashScreenService);
            Argument.IsNotNull(() => ensureStartupService);
            Argument.IsNotNull(() => applicationInitializationService);
            Argument.IsNotNull(() => dependencyResolver);

            _typeFactory = typeFactory;
            _keyboardMappingsService = keyboardMappingsService;
            _commandManager = commandManager;
            _splashScreenService = splashScreenService;
            _ensureStartupService = ensureStartupService;
            _applicationInitializationService = applicationInitializationService;
            _dependencyResolver = dependencyResolver;

            var entryAssembly = AssemblyHelper.GetEntryAssembly();

            Log.Info("Starting {0} v{1} ({2})", entryAssembly.Title(), entryAssembly.Version(), entryAssembly.InformationalVersion());

            // Initialize (now we have an application)
            DotNetPatchHelper.Initialize();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the shell.
        /// </summary>
        /// <value>The shell.</value>
        public IShell Shell { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new shell and shows a splash during the initialization.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        [Time]
        public async Task<TShell> CreateWithSplashAsync<TShell>()
            where TShell : IShell
        {
            await _applicationInitializationService.InitializeBeforeShowingSplashScreenAsync();

            var splashScreen = _splashScreenService.CreateSplashScreen();
            splashScreen.Show();

            var shell = await CreateShellInternalAsync<TShell>(splashScreen.Close);

            return shell;
        }

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        [Time]
        public async Task<TShell> CreateAsync<TShell>()
            where TShell : IShell
        {
            return await CreateShellInternalAsync<TShell>();
        }

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <param name="postShowShellCallback">The shell created callback.</param>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        private async Task<TShell> CreateShellInternalAsync<TShell>(Action postShowShellCallback = null)
            where TShell : IShell
        {
            if (Shell != null)
            {
                throw Log.ErrorAndCreateException<OrchestraException>("The shell is already created and cannot be created again");
            }

            Log.Info("Checking if software was correctly closed previously");

            _ensureStartupService.EnsureFailSafeStartup();

            var shell = default(TShell);
            var successfullyStarted = true;

            try
            {
                await InitializeBeforeCreatingShellAsync();

                shell = await CreateShellAsync<TShell>();

                Log.Info("Loading keyboard mappings");

                await TaskHelper.Run(() => _keyboardMappingsService.Load(), true);

                // Now we have a new window, resubscribe the command manager
                _commandManager.SubscribeToKeyboardEvents();

                await InitializeAfterCreatingShellAsync();

                Log.Info("Confirming that application was started successfully");

                _ensureStartupService.ConfirmApplicationStartedSuccessfully();

                await InitializeBeforeShowingShellAsync();

                ShowShell(shell);

                if (postShowShellCallback != null)
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
                var entryAssembly = AssemblyHelper.GetEntryAssembly();
                var assemblyTitle = entryAssembly.Title();

                // Late resolve so user might change the message service
                var messageService = _dependencyResolver.Resolve<IMessageService>();
                await messageService.ShowErrorAsync(string.Format("An unexpected error occurred while starting {0}. Unfortunately it needs to be closed.\n\nPlease try restarting the application. If this error keeps coming up while starting the application, please contact support.", assemblyTitle), string.Format("Failed to start {0}", assemblyTitle));

                Application.Current.Shutdown(-1);
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

        [Time]
        private async Task<TShell> CreateShellAsync<TShell>()
            where TShell : IShell
        {
            Log.Debug("Creating shell using type '{0}'", typeof(TShell).GetSafeFullName());

            var shell = _typeFactory.CreateInstance<TShell>();
            Shell = shell;

            var shellAsWindow = Shell as Window;
            if (shellAsWindow != null)
            {
                Log.Debug("Setting the new shell as Application.MainWindow");

                var currentApp = Application.Current;
                currentApp.MainWindow = shellAsWindow;
            }

            return shell;
        }

        [Time]
        private void ShowShell(IShell shell)
        {
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
        #endregion
    }
}