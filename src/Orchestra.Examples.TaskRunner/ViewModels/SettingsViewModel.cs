namespace Orchestra.Examples.TaskRunner.ViewModels;

using System;
using System.Threading.Tasks;
using Catel.Fody;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;
using Microsoft.Extensions.Logging;
using Models;

public class SettingsViewModel : FeaturedViewModelBase
{
    //private readonly ILogControlService _logControlService;
    private readonly IDispatcherService _dispatcherService;

    public SettingsViewModel(Settings settings, IServiceProvider serviceProvider, 
        /*ILogControlService logControlService,*/ IDispatcherService dispatcherService)
        : base(serviceProvider)
    {
        Settings = settings;
        //_logControlService = logControlService;
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

        //_dispatcherService.BeginInvoke(() => _logControlService.SelectedLevel = LogLevel.Debug | LogLevel.Information);
    }
}
