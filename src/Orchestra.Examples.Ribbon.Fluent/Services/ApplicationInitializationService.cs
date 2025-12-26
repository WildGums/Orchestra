namespace Orchestra.Examples.Ribbon.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Catel;
    using Catel.MVVM;
    using Microsoft.Extensions.DependencyInjection;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public class ApplicationInitializationService : ApplicationInitializationServiceBase
    {
        public override bool ShowSplashScreen => true;

        public override bool ShowShell => true;

        public ApplicationInitializationService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override async Task InitializeBeforeCreatingShellAsync()
        {
            // Non-async first
            await InitializeCommandsAsync();

            await RunAndWaitAsync(new Func<Task>[]
            {
                InitializePerformanceAsync
            });

            // Note: uncomment to show font size selection at startup
            //var uiVisualizerService = _serviceLocator.GetRequiredService<IUIVisualizerService>();
            //await uiVisualizerService.ShowDialogAsync<FontSizeSelectorViewModel>();
        }

        private async Task InitializeCommandsAsync()
        {
            var splashScreenStatusService = ServiceProvider.GetRequiredService<ISplashScreenStatusService>();
            splashScreenStatusService.UpdateStatus("Initializing commands");

            var commandManager = ServiceProvider.GetRequiredService<ICommandManager>();
            var commandInfoService = ServiceProvider.GetRequiredService<ICommandInfoService>();

            commandManager.CreateCommandWithGesture(typeof(Commands.Application), "Exit");
            commandManager.CreateCommandWithGesture(typeof(Commands.Application), "About");

            commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "LongOperation");
            commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "ShowMessageBox");
            commandManager.CreateCommandWithGesture(typeof(Commands.Demo), "Hidden");
            commandInfoService.UpdateCommandInfo(Commands.Demo.Hidden, x => x.IsHidden = true);

            commandManager.CreateCommand("File.Open", new InputGesture(Key.O, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.SaveToImage", new InputGesture(Key.I, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);
            commandManager.CreateCommand("File.Print", new InputGesture(Key.P, ModifierKeys.Control), throwExceptionWhenCommandIsAlreadyCreated: false);

            var keyboardMappingsService = ServiceProvider.GetRequiredService<IKeyboardMappingsService>();
            keyboardMappingsService.AdditionalKeyboardMappings.Add(new KeyboardMapping("MyGroup.Zoom", "Mousewheel", ModifierKeys.Control));
        }

        public override async Task InitializeAfterCreatingShellAsync()
        {
            var splashScreenStatusService = ServiceProvider.GetRequiredService<ISplashScreenStatusService>();
            splashScreenStatusService.UpdateStatus("Delaying splash screen for demo purposes");

            // Note: use thread.sleep to show a blocking thread but still allows
            // running status updates since the splash screen textblock runs on a
            // separate thread
            Thread.Sleep(2500);
            //await Task.Delay(2500);
        }

        private async Task InitializePerformanceAsync()
        {
            var splashScreenStatusService = ServiceProvider.GetRequiredService<ISplashScreenStatusService>();
            splashScreenStatusService.UpdateStatus("Improving performance");

            await Task.Delay(1000);

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;
        }

        private async Task RegisterTypesAsync()
        {
            var splashScreenStatusService = ServiceProvider.GetRequiredService<ISplashScreenStatusService>();
            splashScreenStatusService.UpdateStatus("Registering types");

            await Task.Delay(1000);

            var serviceLocator = _serviceLocator;

            //throw new Exception("this is a test exception");
        }
    }
}
