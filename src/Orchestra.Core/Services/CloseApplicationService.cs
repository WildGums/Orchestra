namespace Orchestra.Services
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;

    public class CloseApplicationService : ICloseApplicationService
    {
        private readonly IEnsureStartupService _ensureStartupService;

        public CloseApplicationService(IEnsureStartupService ensureStartupService)
        {
            Argument.IsNotNull(() => ensureStartupService);

            _ensureStartupService = ensureStartupService;
        }

        public void Close()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CloseAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public async Task CloseAsync()
        {
            _ensureStartupService.ConfirmApplicationStartedSuccessfully();

            await LogManager.FlushAllAsync();

            // Very dirty, but allow app to write the file
            Thread.Sleep(50);

            Process.GetCurrentProcess().Kill();
        }
    }
}
