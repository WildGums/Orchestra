// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInformationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;

    internal class SystemInformationService : ISystemInformationService
    {
        #region ISystemInformationService Members
        public IEnumerable<string> GetSystemInfo()
        {
            var wmi = new ManagementObjectSearcher("select * from Win32_OperatingSystem")
                .Get()
                .Cast<ManagementObject>()
                .First();

            var cpu = new ManagementObjectSearcher("select * from Win32_Processor")
                .Get()
                .Cast<ManagementObject>()
                .First();

            yield return string.Format("User name: {0}", Environment.UserName);
            yield return string.Format("User domain name: {0}", Environment.UserDomainName);
            yield return string.Format("Machine name: {0}", Environment.MachineName);
            yield return string.Format("OS version: {0}", Environment.OSVersion);
            yield return string.Format("Version: {0}", Environment.Version);

            yield return string.Format("OS name: {0}", GetObjectValue(wmi, "Caption"));
            yield return string.Format("MaxProcessRAM: {0}", GetObjectValue(wmi, "MaxProcessMemorySize"));
            yield return string.Format("Architecture: {0}", GetObjectValue(wmi, "OSArchitecture"));
            yield return string.Format("ProcessorId: {0}", GetObjectValue(wmi, "ProcessorId"));
            yield return string.Format("Build: {0}", GetObjectValue(wmi, "BuildNumber"));
            yield return string.Format(string.Empty);

            yield return string.Format("CPU name: {0}", GetObjectValue(cpu, "Name"));
            yield return string.Format("Description: {0}", GetObjectValue(cpu, "Caption"));
            yield return string.Format("Address width: {0}", GetObjectValue(cpu, "AddressWidth"));
            yield return string.Format("Data width: {0}", GetObjectValue(cpu, "DataWidth"));
            yield return string.Format("SpeedMHz: {0}", GetObjectValue(cpu, "MaxClockSpeed"));
            yield return string.Format("BusSpeedMHz: {0}", GetObjectValue(cpu, "ExtClock"));
            yield return string.Format("Number of cores: {0}", GetObjectValue(cpu, "NumberOfCores"));
            yield return string.Format("Number of logical processors: {0}", GetObjectValue(cpu, "NumberOfLogicalProcessors"));
        }
        #endregion

        #region Methods
        private string GetObjectValue(ManagementObject obj, string key)
        {
            try
            {
                return obj[key].ToString();
            }
            catch (ManagementException)
            {
                return "n/a";
            }
            catch (Exception)
            {
                return "n/a";
            }
        }
        #endregion
    }
}