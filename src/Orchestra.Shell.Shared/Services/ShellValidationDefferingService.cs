namespace Orchestra.Services
{
    public class ShellValidationDefferingService : IShellValidationDefferingService
    {
        public bool DeferValidationUntilFirstSaveCall { get; set; } = true;
    }
}
