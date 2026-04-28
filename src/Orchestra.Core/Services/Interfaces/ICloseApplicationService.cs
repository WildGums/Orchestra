namespace Orchestra;

using System.Threading.Tasks;

public interface ICloseApplicationService
{
    Task CloseAsync();
    Task CloseAsync(bool force);
}
