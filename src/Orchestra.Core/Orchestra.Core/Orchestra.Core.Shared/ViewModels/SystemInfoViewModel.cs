// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfoViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Threading;
    using Orc.SystemInfo;
    using Services;

    public class SystemInfoViewModel : ViewModelBase
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly IDispatcherService _dispatcherService;
        private readonly IClipboardService _clipboardService;

        #region Constructors
        public SystemInfoViewModel(ISystemInfoService systemInfoService, IDispatcherService dispatcherService, IClipboardService clipboardService)
        {
            Argument.IsNotNull(() => systemInfoService);
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => clipboardService);

            _systemInfoService = systemInfoService;
            _dispatcherService = dispatcherService;
            _clipboardService = clipboardService;

            SystemInfo = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(string.Empty, string.Empty) };

            CopyToClipboard = new Command(OnCopyToClipboardExecute);
        }
        #endregion

        #region Properties
        public List<KeyValuePair<string, string>> SystemInfo { get; private set; }

        public bool IsSystemInformationLoaded { get; private set; }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var items = new List<KeyValuePair<string, string>>();

            var systemInfo = await TaskHelper.Run(() => _systemInfoService.GetSystemInfo(), true);

            foreach (var item in systemInfo)
            {
                items.Add(new KeyValuePair<string, string>(item.Name, item.Value));
            }

            SystemInfo = items;
            IsSystemInformationLoaded = true;
        }
        #endregion

        #region Commands
        public Command CopyToClipboard { get; private set; }

        private void OnCopyToClipboardExecute()
        {
            var sb = new StringBuilder();

            foreach (var item in SystemInfo)
            {
                sb.AppendFormat("{0} {1} {2}", item.Key, item.Value, Environment.NewLine);
            }

            _clipboardService.CopyToClipboard(sb.ToString());
        }
        #endregion
    }
}