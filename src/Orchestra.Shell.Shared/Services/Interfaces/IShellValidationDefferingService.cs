namespace Orchestra.Services
{
    public interface IShellValidationDefferingService
    {
        public bool DeferValidationUntilFirstSaveCall { get; set; }
    }
}
