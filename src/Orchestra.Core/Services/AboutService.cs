namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using Catel.Services;
    using Microsoft.Extensions.Logging;
    using ViewModels;

    public class AboutService : IAboutService
    {
        private readonly ILogger<AboutService> _logger;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IAboutInfoService _aboutInfoService;

        public AboutService(ILogger<AboutService> logger, IUIVisualizerService uiVisualizerService, 
            IAboutInfoService aboutInfoService)
        {
            _logger = logger;
            _uiVisualizerService = uiVisualizerService;
            _aboutInfoService = aboutInfoService;
        }

        public virtual async Task ShowAboutAsync()
        {
            var aboutInfo = await _aboutInfoService.GetAboutInfoAsync();
            if (aboutInfo is not null)
            {
                _logger.LogInformation("Showing about dialog");

                await _uiVisualizerService.ShowDialogAsync<AboutViewModel>(aboutInfo);
            }
            else
            {
                _logger.LogWarning("IAboutInfoService.GetAboutInfo() returned null, cannot show about window");
            }
        }
    }
}
