namespace Orchestra.Services
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public class CloseApplicationService : ICloseApplicationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IEnsureStartupService _ensureStartupService;
        private readonly IMainWindowService _mainWindowService;

        public CloseApplicationService(IEnsureStartupService ensureStartupService, IMainWindowService mainWindowService)
        {
            Argument.IsNotNull(() => ensureStartupService);
            Argument.IsNotNull(() => mainWindowService);

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
            _ensureStartupService.ConfirmApplicationStartedSuccessfully();

            await LogManager.FlushAllAsync();

            if (!force)
            {
                // Close via main window
                // Close via main window
                var mainWindow = await _mainWindowService.GetMainWindowAsync();
                if (mainWindow is not null)
                {
                    Log.Debug("Handling closing of app via main window");

                    mainWindow.Close();
                    return;
                }
            }

            try
            {
                Log.Debug("Allowing all close application watchers to run their ClosedAsync methods");

                CloseApplicationWatcherBase.SkipClosing = force;
                await CloseApplicationWatcherBase.PerformClosedOperationsAsync();
            }
            catch (Exception)
            {
                // Always continue closing the app
            }

            Log.Debug("Handling closing of app via process");

            await LogManager.FlushAllAsync();

            // Very dirty, but allow app to write the file
            Thread.Sleep(50);

            Process.GetCurrentProcess().Kill();
        }
    }
}
