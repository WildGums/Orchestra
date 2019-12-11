namespace Orchestra.Services
{
    public class ShellConfigurationService : IShellConfigurationService
    {
        public virtual bool DeferValidationUntilFirstSaveCall { get; set; }
    }
}
