// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;
    using Catel;
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
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => aboutInfoService);

            _uiVisualizerService = uiVisualizerService;
            _aboutInfoService = aboutInfoService;
        }

        public virtual async Task ShowAboutAsync()
        {
            var aboutInfo = _aboutInfoService.GetAboutInfo();
            if (aboutInfo != null)
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