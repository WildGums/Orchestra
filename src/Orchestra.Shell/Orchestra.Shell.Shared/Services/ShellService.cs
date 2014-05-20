// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Reflection;
    using Views;

    public class ShellService : IShellService
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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellService" /> class.
        /// </summary>
        /// <param name="typeFactory">The type factory.</param>
        /// <param name="commandManager">The command manager.</param>
        /// <param name="keyboardMappingsService">The keyboard mappings service.</param>
        /// <param name="splashScreenService"></param>
        /// <exception cref="ArgumentNullException">The <paramref name="typeFactory" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="commandManager" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="keyboardMappingsService" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="splashScreenService" /> is <c>null</c>.</exception>
        public ShellService(ITypeFactory typeFactory, ICommandManager commandManager, IKeyboardMappingsService keyboardMappingsService,
            ISplashScreenService splashScreenService)
        {
            Argument.IsNotNull("typeFactory", typeFactory);
            Argument.IsNotNull("commandManager", commandManager);
            Argument.IsNotNull("keyboardMappingsService", keyboardMappingsService);
            Argument.IsNotNull("splashScreenService", splashScreenService);

            _typeFactory = typeFactory;
            _commandManager = commandManager;
            _keyboardMappingsService = keyboardMappingsService;
            _splashScreenService = splashScreenService;
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
        /// <param name="preInitialize">The pre initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <param name="initializeCommands">The initialize commands handler. If <c>null</c>, no commands will be initialized.</param>
        /// <param name="postInitialize">The post initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        public TShell CreateWithSplash<TShell>(Action preInitialize = null, Action<ICommandManager> initializeCommands = null, Action postInitialize = null)
            where TShell : IShell
        {
            var splashScreen = _splashScreenService.CreateSplashScreen();
            splashScreen.Show();

            var shell = Create<TShell>(preInitialize, initializeCommands, postInitialize);

            splashScreen.Close();

            return shell;
        }

        /// <summary>
        /// Creates a new shell.
        /// </summary>
        /// <typeparam name="TShell">The type of the shell.</typeparam>
        /// <param name="preInitialize">The pre initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <param name="initializeCommands">The initialize commands handler. If <c>null</c>, no commands will be initialized.</param>
        /// <param name="postInitialize">The post initialize handler to initialize custom logic. If <c>null</c>, this value will be ignored.</param>
        /// <returns>The created shell.</returns>
        /// <exception cref="OrchestraException">The shell is already created and cannot be created again.</exception>
        public TShell Create<TShell>(Action preInitialize = null, Action<ICommandManager> initializeCommands = null, Action postInitialize = null)
            where TShell : IShell
        {
            if (Shell != null)
            {
                Log.ErrorAndThrowException<OrchestraException>("The shell is already created and cannot be created again");
            }

            if (preInitialize != null)
            {
                Log.Debug("Calling pre initialize");

                preInitialize();
            }

            if (initializeCommands != null)
            {
                Log.Info("Initializing commands");

                initializeCommands(_commandManager);
            }

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

            Log.Info("Loading keyboard mappings");

            _keyboardMappingsService.Load();

            // Now we have a new window, resubscribe the command manager
            _commandManager.SubscribeToKeyboardEvents();

            if (postInitialize != null)
            {
                Log.Debug("Calling post initialize");

                postInitialize();
            }

            shell.Show();

            return shell;
        }
        #endregion
    }
}