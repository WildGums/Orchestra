namespace Orchestra.Services
{
    using System.Threading.Tasks;

    public partial interface IShellRecoveryService
    {
        Task StartRecoveryAsync(ShellRecoveryContext shellRecoveryContext);
    }
}
