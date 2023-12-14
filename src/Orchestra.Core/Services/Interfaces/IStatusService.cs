namespace Orchestra.Services
{
    using Orc.Controls.Services;

    public interface IStatusService
    {
        void Initialize(IStatusRepresenter statusRepresenter);
        void UpdateStatus(string status);
    }
}
