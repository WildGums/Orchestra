namespace Orchestra.Services
{
    using Orc.Controls.Services;

    public interface ISplashScreenStatusService
    {
        void Initialize(IStatusRepresenter statusRepresenter);
        void UpdateStatus(string status);
    }
}
