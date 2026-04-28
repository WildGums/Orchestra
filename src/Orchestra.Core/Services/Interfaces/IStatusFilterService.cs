namespace Orchestra;

public interface IStatusFilterService
{
    bool IsSuspended { get; set; }
    
    string? GetStatus(string status);
}
