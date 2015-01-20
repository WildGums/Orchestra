// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfoViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Text;
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Services;

    public class SystemInfoViewModel : ViewModelBase
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly IDispatcherService _dispatcherService;

        #region Constructors
        public SystemInfoViewModel(ISystemInfoService systemInfoService, IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => systemInfoService);
            Argument.IsNotNull(() => dispatcherService);

            _systemInfoService = systemInfoService;
            _dispatcherService = dispatcherService;

            SystemInfo = "Retrieving system info...";
        }
        #endregion

        #region Properties
        public string SystemInfo { get; private set; }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

            var sb = new StringBuilder();

            await Task.Factory.StartNew(() =>
            {
                foreach (var line in _systemInfoService.GetSystemInfo())
                {
                    sb.AppendLine(line);
                }
            });

            SystemInfo = sb.ToString();
        }
        #endregion
    }
}