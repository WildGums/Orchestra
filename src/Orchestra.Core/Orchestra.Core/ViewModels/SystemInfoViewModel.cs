// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfoViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Text;
    using Catel;
    using Catel.MVVM;
    using Services;

    public class SystemInfoViewModel : ViewModelBase
    {
        #region Constructors
        public SystemInfoViewModel(ISystemInfoService systemInfoService)
        {
            Argument.IsNotNull(() => systemInfoService);

            var sb = new StringBuilder();
            foreach (var line in systemInfoService.GetSystemInfo())
            {
                sb.AppendLine(line);
            }
            SystemInfo = sb.ToString();
        }
        #endregion

        #region Properties
        public string SystemInfo { get; private set; }
        #endregion
    }
}