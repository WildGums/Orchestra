namespace Orchestra.Services
{
    using MahApps.Metro.Controls;

    public interface IMahAppsService : IShellContentService, IAboutInfoService
    {
        WindowCommands GetRightWindowCommands();
    }
}
