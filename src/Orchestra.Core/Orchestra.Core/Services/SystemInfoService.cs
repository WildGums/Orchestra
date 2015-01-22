// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemInfoService.cs" company="Orchestra development team">
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
    using Microsoft.Win32;

    internal class SystemInfoService : ISystemInfoService
    {
        #region ISystemInfoService Members
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

            yield return new KeyValuePair<string, string>("Current culture:", CultureInfo.CurrentCulture.ToString());

            yield return new KeyValuePair<string, string>("Installed .Net versions:", string.Empty);

            var versions = GetVersionFromRegistry();
            foreach (var version in versions)
            {
                yield return new KeyValuePair<string, string>(string.Empty, version);
            }
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

        private static IEnumerable<string> GetVersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                    OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5  
                // or later, you can use: 
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,  
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {
                        var versionKey = ndpKey.OpenSubKey(versionKeyName);
                        var name = (string) versionKey.GetValue("Version", "");
                        var sp = versionKey.GetValue("SP", "").ToString();
                        var install = versionKey.GetValue("Install", "").ToString();
                        if (sp != "" && install == "1")
                        {
                            yield return (versionKeyName + "  " + name + "  SP" + sp);
                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            var subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string) subKey.GetValue("Version", "");
                            if (name != "")
                            {
                                sp = subKey.GetValue("SP", "").ToString();
                            }
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "") //no install info, must be later.
                            {
                                yield return (versionKeyName + "  " + name);
                            }
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    yield return (versionKeyName + "  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    yield return (versionKeyName + "  " + subKeyName + "  " + name);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}