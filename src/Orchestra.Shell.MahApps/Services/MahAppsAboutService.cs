namespace Orchestra.Services
{
    using Catel.Services;
    using ViewModels;
    using Views;

    public class MahAppsAboutService : AboutService
    {
        public MahAppsAboutService(IUIVisualizerService uiVisualizerService, IAboutInfoService aboutInfoService)
            : base(uiVisualizerService, aboutInfoService)
        {
            uiVisualizerService.Register(typeof(AboutViewModel), typeof(MahAppsAboutView));
        }
    }
}
