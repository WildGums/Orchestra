namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Logging;
    using Catel.Reflection;
    using Catel.Services;

    public partial class ShellRecoveryService : IShellRecoveryService
    {
        private readonly IMessageService _messageService;

        public ShellRecoveryService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task StartRecoveryAsync(ShellRecoveryContext shellRecoveryContext)
        {
            var entryAssembly = Catel.Reflection.AssemblyHelper.GetRequiredEntryAssembly();
            var assemblyTitle = entryAssembly.Title();

            await _messageService.ShowErrorAsync(string.Format("An unexpected error occurred while starting {0}. Unfortunately it needs to be closed.\n\nPlease try restarting the application. If this error keeps coming up while starting the application, please contact support.", assemblyTitle), string.Format("Failed to start {0}", assemblyTitle));

            Application.Current.Shutdown(-1);
        }
    }
}
