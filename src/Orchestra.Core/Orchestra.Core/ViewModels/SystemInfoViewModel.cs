// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfoViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Collections.Generic;
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

            SystemInfo = new List<KeyValuePair<string, string>> {new KeyValuePair<string, string>("Retrieving system info...", string.Empty)};
        }
        #endregion

        #region Properties
        public List<KeyValuePair<string, string>> SystemInfo { get; private set; }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

            var container = new List<KeyValuePair<string, string>>();

            await Task.Factory.StartNew(() =>
            {
                foreach (var line in _systemInfoService.GetSystemInfo())
                {
                    container.Add(line);
                }
            });

            SystemInfo = container;
        }
        #endregion
    }
}