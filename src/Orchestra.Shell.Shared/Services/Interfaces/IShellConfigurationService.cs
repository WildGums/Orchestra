namespace Orchestra;

public interface IShellConfigurationService
{
    public bool ValidateUsingDataAnnotations { get; set; }
    public bool DeferValidationUntilFirstSaveCall { get; set; }
}
