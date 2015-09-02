// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsAboutService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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