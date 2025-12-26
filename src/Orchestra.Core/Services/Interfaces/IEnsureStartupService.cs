namespace Orchestra
{
    using System.Threading.Tasks;

    public interface IEnsureStartupService
    {
        bool SuccessfullyStarted { get; }

        Task ConfirmApplicationStartedSuccessfullyAsync();
        Task EnsureFailSafeStartupAsync();
    }
}
