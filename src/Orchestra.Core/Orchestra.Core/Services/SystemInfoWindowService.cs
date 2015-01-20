// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfoService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Catel;
    using Catel.Logging;
    using Catel.Services;
    using ViewModels;

    public class SystemInfoWindowService : ISystemInfoService
    {
        #region Constants
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly ISystemInformationService _systemInformationService;
        private readonly IUIVisualizerService _uiVisualizerService;
        #endregion

        #region Constructors
        public SystemInfoWindowService(IUIVisualizerService uiVisualizerService, ISystemInformationService systemInformationService)
        {
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => systemInformationService);

            _uiVisualizerService = uiVisualizerService;
            _systemInformationService = systemInformationService;
        }
        #endregion

        #region ISystemInfoService Members
        public virtual void ShowSystemInfo()
        {
            var systemInfo = _systemInformationService.GetSystemInfo();
            if (systemInfo != null)
            {
                Log.Info("Showing system info dialog");

                _uiVisualizerService.ShowDialog<SystemInfoViewModel>(systemInfo);
            }
            else
            {
                Log.Warning("ISystemInformationService.GetSystemInfo() returned null, cannot show about window");
            }
        }
        #endregion
    }
}