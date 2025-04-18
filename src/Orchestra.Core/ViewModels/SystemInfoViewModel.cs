namespace Orchestra.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orc.SystemInfo;
    using Services;

    public class SystemInfoViewModel : ViewModelBase
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly IClipboardService _clipboardService;

        public SystemInfoViewModel(ISystemInfoService systemInfoService, IClipboardService clipboardService)
        {
            ArgumentNullException.ThrowIfNull(systemInfoService);
            ArgumentNullException.ThrowIfNull(clipboardService);

            _systemInfoService = systemInfoService;
            _clipboardService = clipboardService;

            ValidateUsingDataAnnotations = false;

            SystemInfo = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(string.Empty, string.Empty) };

            CopyToClipboard = new Command(OnCopyToClipboardExecute);
        }

        public List<KeyValuePair<string, string>> SystemInfo { get; private set; }

        public bool IsSystemInformationLoaded { get; private set; }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var items = new List<KeyValuePair<string, string>>();

            var systemInfo = await Task.Run(() => _systemInfoService.GetSystemInfo());

            foreach (var item in systemInfo)
            {
                items.Add(new KeyValuePair<string, string>(item.Name, item.Value));
            }

            SystemInfo = items;
            IsSystemInformationLoaded = true;
        }

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
    }
}
