namespace Orchestra.Win32
{
    using System;

    [Flags]
    public enum DpiAwarenessContext
    {
        /// <summary>
        ///     DPI unaware. This window does not scale for DPI changes and is always assumed to have a scale factor of 100% (96 DPI). 
        ///     It will be automatically scaled by the system on any other DPI setting.
        /// </summary>
        Unaware = 0,

        /// <summary>
        ///     System DPI aware. This window does not scale for DPI changes. 
        ///     It will query for the DPI once and use that value for the lifetime of the process. If the DPI changes, the process will not adjust to the new DPI value. 
        ///     It will be automatically scaled up or down by the system when the DPI changes from the system value.
        /// </summary>
        System = 1,

        /// <summary>
        ///     Per monitor DPI aware. This window checks for the DPI when it is created and adjusts the scale factor whenever the DPI changes. 
        ///     These processes are not automatically scaled by the system.
        /// </summary>
        ProcessPerMonitor = 2,

        /// <summary>
        ///     Also known as Per Monitor v2. An advancement over the original per-monitor DPI awareness mode, 
        ///     which enables applications to access new DPI-related scaling behaviors on a per top-level window basis.
        /// </summary>
        ProcessPerMonitorV2 = 3,

        /// <summary>
        ///     DPI unaware with improved quality of GDI-based content. 
        ///     This mode behaves similarly to DPI_AWARENESS_CONTEXT_UNAWARE, but also enables the system to automatically improve the 
        ///     rendering quality of text and other GDI-based primitives when the window is displayed on a high-DPI monitor.
        /// </summary>
        UnawareGdiScaled = 4
    }
}
