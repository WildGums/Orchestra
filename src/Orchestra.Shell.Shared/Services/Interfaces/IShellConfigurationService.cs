namespace Orchestra.Services
{
    public interface IShellConfigurationService
    {
        public bool DeferValidationUntilFirstSaveCall { get; set; }
    }
}
