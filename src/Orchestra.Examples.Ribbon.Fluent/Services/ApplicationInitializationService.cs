namespace Orchestra.Examples.Ribbon.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Catel;
    using Catel.Configuration;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Orchestra.Examples.Ribbon.ViewModels;
    using Orchestra.Services;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        private readonly IServiceLocator _serviceLocator;

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        
        public override bool ShowSplashScreen => true;

        public override bool ShowShell => true;

        public ApplicationInitializationService(IServiceLocator serviceLocator)
        {
            ArgumentNullException.ThrowIfNull(serviceLocator);

            _serviceLocator = serviceLocator;
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

            var uiVisualizerService = _serviceLocator.ResolveRequiredType<IUIVisualizerService>();
            await uiVisualizerService.ShowDialogAsync<FontSizeSelectorViewModel>();
        }

        private async Task InitializeCommandsAsync()
        {
            var commandManager = ServiceLocator.Default.ResolveRequiredType<ICommandManager>();
            var commandInfoService = ServiceLocator.Default.ResolveRequiredType<ICommandInfoService>();

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
            Log.Info("Delay to show the splash screen");

            Thread.Sleep(2500);
        }

        private async Task InitializePerformanceAsync()
        {
            Log.Info("Improving performance");

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private async Task RegisterTypesAsync()
        {
            var serviceLocator = _serviceLocator;

            serviceLocator.RegisterType<IAboutInfoService, AboutInfoService>();
            serviceLocator.RegisterTypeAndInstantiate<UserMessageCloseApplicationWatcher>();

            //throw new Exception("this is a test exception");
        }
    }
}
