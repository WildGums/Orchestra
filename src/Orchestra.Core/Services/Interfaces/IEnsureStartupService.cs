namespace Orchestra.Services
{
    using System.Threading.Tasks;

    public interface IEnsureStartupService
    {
        bool SuccessfullyStarted { get; }

        Task ConfirmApplicationStartedSuccessfullyAsync();
        Task EnsureFailSafeStartupAsync();
    }
}
