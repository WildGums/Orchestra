namespace Orchestra;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Catel;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class CloseApplicationService : ICloseApplicationService
{
    private readonly ILogger<CloseApplicationService> _logger;
    private readonly IEnsureStartupService _ensureStartupService;
    private readonly IMainWindowService _mainWindowService;

    public CloseApplicationService(ILogger<CloseApplicationService> logger, 
        IEnsureStartupService ensureStartupService, IMainWindowService mainWindowService)
    {
        _logger = logger;
        _ensureStartupService = ensureStartupService;
        _mainWindowService = mainWindowService;
    }

    public void Close()
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        CloseAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    public Task CloseAsync()
    {
        return CloseAsync(true);
    }

    public async Task CloseAsync(bool force)
    {
        await _ensureStartupService.ConfirmApplicationStartedSuccessfullyAsync();

        //await LogManager.FlushAllAsync();

        if (!force)
        {
            // Close via main window
            // Close via main window
            var mainWindow = await _mainWindowService.GetMainWindowAsync();
            if (mainWindow is not null)
            {
                _logger.LogDebug("Handling closing of app via main window");

                mainWindow.Close();
                return;
            }
        }

        try
        {
            _logger.LogDebug("Allowing all close application watchers to run their ClosedAsync methods");

            CloseApplicationWatcherBase.SkipClosing = force;
            await CloseApplicationWatcherBase.PerformClosedOperationsAsync();
        }
        catch (Exception)
        {
            // Always continue closing the app
        }

        _logger.LogDebug("Handling closing of app via process");

        //await LogManager.FlushAllAsync();

        // Very dirty, but allow app to write the file
        Thread.Sleep(50);

        Process.GetCurrentProcess().Kill();
    }
}
