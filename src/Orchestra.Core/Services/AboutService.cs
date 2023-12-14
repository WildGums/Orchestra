namespace Orchestra.Services
{
    using System;
    using System.Threading.Tasks;
    using Catel.Logging;
    using Catel.Services;
    using ViewModels;

    public class AboutService : IAboutService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IAboutInfoService _aboutInfoService;

        public AboutService(IUIVisualizerService uiVisualizerService, IAboutInfoService aboutInfoService)
        {
            ArgumentNullException.ThrowIfNull(uiVisualizerService);
            ArgumentNullException.ThrowIfNull(aboutInfoService);

            _uiVisualizerService = uiVisualizerService;
            _aboutInfoService = aboutInfoService;
        }

        public virtual async Task ShowAboutAsync()
        {
            var aboutInfo = await _aboutInfoService.GetAboutInfoAsync();
            if (aboutInfo is not null)
            {
                Log.Info("Showing about dialog");

                await _uiVisualizerService.ShowDialogAsync<AboutViewModel>(aboutInfo);
            }
            else
            {
                Log.Warning("IAboutInfoService.GetAboutInfo() returned null, cannot show about window");
            }
        }
    }
}
