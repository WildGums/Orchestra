// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInformationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Management;

    internal class SystemInfoService : ISystemInfoService
    {
        #region ISystemInformationService Members
        public IEnumerable<KeyValuePair<string, string>> GetSystemInfo()
        {
            var wmi = new ManagementObjectSearcher("select * from Win32_OperatingSystem")
                .Get()
                .Cast<ManagementObject>()
                .First();

            var cpu = new ManagementObjectSearcher("select * from Win32_Processor")
                .Get()
                .Cast<ManagementObject>()
                .First();


            yield return new KeyValuePair<string, string>("User name:", Environment.UserName);
            yield return new KeyValuePair<string, string>("User domain name:", Environment.UserDomainName);
            yield return new KeyValuePair<string, string>("Machine name:", Environment.MachineName);
            yield return new KeyValuePair<string, string>("OS version:", Environment.OSVersion.ToString());
            yield return new KeyValuePair<string, string>("Version:", Environment.Version.ToString());

            yield return new KeyValuePair<string, string>("OS name:", GetObjectValue(wmi, "Caption"));
            yield return new KeyValuePair<string, string>("MaxProcessRAM:", GetObjectValue(wmi, "MaxProcessMemorySize"));
            yield return new KeyValuePair<string, string>("Architecture:", GetObjectValue(wmi, "OSArchitecture"));
            yield return new KeyValuePair<string, string>("ProcessorId:", GetObjectValue(wmi, "ProcessorId"));
            yield return new KeyValuePair<string, string>("Build:", GetObjectValue(wmi, "BuildNumber"));

            yield return new KeyValuePair<string, string>("CPU name:", GetObjectValue(cpu, "Name"));
            yield return new KeyValuePair<string, string>("Description:", GetObjectValue(cpu, "Caption"));
            yield return new KeyValuePair<string, string>("Address width:", GetObjectValue(cpu, "AddressWidth"));
            yield return new KeyValuePair<string, string>("Data width:", GetObjectValue(cpu, "DataWidth"));
            yield return new KeyValuePair<string, string>("SpeedMHz:", GetObjectValue(cpu, "MaxClockSpeed"));
            yield return new KeyValuePair<string, string>("BusSpeedMHz:", GetObjectValue(cpu, "ExtClock"));
            yield return new KeyValuePair<string, string>("Number of cores:", GetObjectValue(cpu, "NumberOfCores"));
            yield return new KeyValuePair<string, string>("Number of logical processors:", GetObjectValue(cpu, "NumberOfLogicalProcessors"));
        }
        #endregion

        #region Methods
        private string GetObjectValue(ManagementObject obj, string key)
        {
            var finalValue = "n/a";

            try
            {
                var value = obj[key];
                if (value != null)
                {
                    finalValue = value.ToString();
                }
            }
            catch (ManagementException)
            {
            }
            catch (Exception)
            {
            }

            return finalValue;
        }
        #endregion
    }
}