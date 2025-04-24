namespace Orchestra.Services
{
    public class ShellConfigurationService : IShellConfigurationService
    {
        public ShellConfigurationService()
        {
            ValidateUsingDataAnnotations = false;
        }

        public virtual bool ValidateUsingDataAnnotations { get; set; }
        public virtual bool DeferValidationUntilFirstSaveCall { get; set; }
    }
}
