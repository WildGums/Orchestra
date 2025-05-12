namespace Orchestra.Examples.Ribbon.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Orchestra.Services;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IServiceLocator _serviceLocator;
        private readonly ISplashScreenStatusService _splashScreenStatusService;
        
        public override bool ShowSplashScreen => true;

        public override bool ShowShell => true;

        public ApplicationInitializationService(IServiceLocator serviceLocator,
            ISplashScreenStatusService splashScreenStatusService)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);

            _serviceLocator = serviceLocator;
            _splashScreenStatusService = splashScreenStatusService;
        }

        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
            await RegisterTypesAsync();
            await InitializeCommandsAsync();

            await RunAndWaitAsync(new Func<Task>[]
            {
                InitializePerformanceAsync
            });

            // Note: uncomment to show font size selection at startup
            //var uiVisualizerService = _serviceLocator.ResolveRequiredType<IUIVisualizerService>();
            //await uiVisualizerService.ShowDialogAsync<FontSizeSelectorViewModel>();
        }

        private async Task InitializeCommandsAsync()
        {
            _splashScreenStatusService.UpdateStatus("Initializing commands");

            var commandManager = _serviceLocator.ResolveRequiredType<ICommandManager>();
            var commandInfoService = _serviceLocator.ResolveRequiredType<ICommandInfoService>();

            commandManager.CreateCommandWithGesture(typeof(Commands.Application), "Exit");
            commandManager.CreateCommandWithGesture(typeof(Commands.Application), "About");

            commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "LongOperation");
            commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "ShowMessageBox");
            commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "Hidden");
            commandInfoService.UpdateCommandInfo(Commands.Demo.Hidden, x => x.IsHidden = true);

            commandManager.CreateCommand("File.Open", new InputGesture(Key.O, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.SaveToImage", new InputGesture(Key.I, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Print", new InputGesture(Key.P, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);

            var keyboardMappingsService = _serviceLocator.ResolveRequiredType<IKeyboardMappingsService>();
            keyboardMappingsService.AdditionalKeyboardMappings.Add(new KeyboardMapping("MyGroup.Zoom", "Mousewheel", ModifierKeys.Control));
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            _splashScreenStatusService.UpdateStatus("Delaying splash screen for demo purposes");

            // Note: use thread.sleep to show a blocking thread but still allows
            // running status updates since the splash screen textblock runs on a
            // separate thread
            Thread.Sleep(2500);
            //await Task.Delay(2500);
        }

        private async Task InitializePerformanceAsync()
        {
            _splashScreenStatusService.UpdateStatus("Improving performance");

            await Task.Delay(1000);

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private async Task RegisterTypesAsync()
        {
            _splashScreenStatusService.UpdateStatus("Registering types");

            await Task.Delay(1000);

            var serviceLocator = _serviceLocator;

            serviceLocator.RegisterType<IAboutInfoService, AboutInfoService>();
            serviceLocator.RegisterTypeAndInstantiate<UserMessageCloseApplicationWatcher>();

            //throw new Exception("this is a test exception");
        }
    }
}
