namespace Orchestra.Win32
{
    using System;

    [Flags]
    public enum DpiAwareness
    {
        /// <summary>
        ///     DPI unaware. This process does not scale for DPI changes and is always assumed to have a scale factor of 100% (96 DPI). 
        ///     It will be automatically scaled by the system on any other DPI setting.
        /// </summary>
        Unaware = 0x0,

        /// <summary>
        ///     System DPI aware. This process does not scale for DPI changes. 
        ///     It will query for the DPI once and use that value for the lifetime of the process. If the DPI changes, the process will not adjust to the new DPI value. 
        ///     It will be automatically scaled up or down by the system when the DPI changes from the system value.
        /// </summary>
        System = 0x1,

        /// <summary>
        ///     Per monitor DPI aware. This process checks for the DPI when it is created and adjusts the scale factor whenever the DPI changes. 
        ///     These processes are not automatically scaled by the system.
        /// </summary>
        ProcessPerMonitor = 0x2
    }
}
