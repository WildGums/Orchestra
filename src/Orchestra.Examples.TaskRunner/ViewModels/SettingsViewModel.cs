namespace Orchestra.Examples.TaskRunner.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel.Fody;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orchestra.Services;

    public class SettingsViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ILogControlService _logControlService;
        private readonly IDispatcherService _dispatcherService;

        public SettingsViewModel(Settings settings, ILogControlService logControlService, IDispatcherService dispatcherService)
        {
            ArgumentNullException.ThrowIfNull(settings);
            ArgumentNullException.ThrowIfNull(logControlService);
            ArgumentNullException.ThrowIfNull(dispatcherService);

            Settings = settings;
            _logControlService = logControlService;
            _dispatcherService = dispatcherService;
        }

        [Model]
        [Expose("OutputDirectory")]
        [Expose("WorkingDirectory")]
        [Expose("CurrentTime")]
        [Expose("HorizonStart")]
        [Expose("HorizonEnd")]
        public Settings Settings { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _dispatcherService.BeginInvoke(() => _logControlService.SelectedLevel = LogEvent.Debug | LogEvent.Info);
        }
    }
}
