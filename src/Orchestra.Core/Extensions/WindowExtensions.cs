// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using Catel;
    using Catel.Logging;
    using Catel.Windows;
    using Orc.Controls;
    using Window = System.Windows.Window;
    
    public static partial class WindowExtensions
    {
        #region Win32
        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
        
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool BringWindowToTop(HandleRef hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        private static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);
        #endregion

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void DisableCloseButton(this Window window)
        {
            if (!window.IsVisible)
            {
                window.SourceInitialized += OnWindowInitializedForDisableCloseButton;
                return;
            }

            var windowInteropHelper = new WindowInteropHelper(window);

            var sysMenu = GetSystemMenu(windowInteropHelper.Handle, false);
            EnableMenuItem(sysMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
        }

        private static void OnWindowInitializedForDisableCloseButton(object sender, EventArgs e)
        {
            var window = (Window) sender;
            window.SourceInitialized -= OnWindowInitializedForDisableCloseButton;

            DisableCloseButton(window);
        }

        public static void SetMaximumWidthAndHeight(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            window.SetMaximumWidth();
            window.SetMaximumHeight();
        }

        public static void SetMaximumWidth(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            window.SetCurrentValue(FrameworkElement.MaxWidthProperty, SystemParameters.WorkArea.Width - 40);
        }

        public static void SetMaximumHeight(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            window.SetCurrentValue(FrameworkElement.MaxHeightProperty, SystemParameters.WorkArea.Height - 40);
        }

        public static void CenterWindowToScreen(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            window.CenterWindowToSize(new Rect(0, 0, screenWidth, screenHeight));
        }

        /// <summary>
        /// Activates the window this framework element contains to.
        /// </summary>
        /// <param name="frameworkElement">Reference to the current <see cref="FrameworkElement"/>.</param>
        public static void BringWindowToTop(this FrameworkElement frameworkElement)
        {
            if (frameworkElement is null)
            {
                return;
            }

            try
            {
                // Get the handle (of the window or process)
                var ownerWindow = frameworkElement.FindVisualAncestorByType<Window>();
                var windowHandle = (ownerWindow is not null) ? new WindowInteropHelper(ownerWindow).Handle : Process.GetCurrentProcess().MainWindowHandle;
                if (windowHandle != IntPtr.Zero)
                {
                    SetForegroundWindowEx(windowHandle);
                }
            }
            catch (Exception)
            {
                // Ignore
            }
        }

        /// <summary>
        /// Sets the foreground window (some "dirty" win32 stuff).
        /// </summary>
        /// <param name="hWnd">Handle of the window to set to the front.</param>
        /// <remarks>
        /// This method takes over the input thread for the window. This means that you are unable
        /// to debug the code between "Attach" and "Detach" since the input thread of Visual Studio
        /// will be attached to the thread of the application.
        /// </remarks>
        private static void SetForegroundWindowEx(IntPtr hWnd)
        {
            var foregroundWindowThreadID = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            var currentThreadID = GetCurrentThreadId();

            if (!AttachThreadInput(foregroundWindowThreadID, currentThreadID, true))
            {
                Log.Warning("Failed to attach to input thread (Win32 code '{0}')", Marshal.GetLastWin32Error());
                return;
            }

            var lastActivePopupWindow = GetLastActivePopup(hWnd);
            SetActiveWindow(lastActivePopupWindow);

            if (!AttachThreadInput(foregroundWindowThreadID, currentThreadID, false))
            {
                Log.Warning("Failed to detach from input thread");
                return;
            }

            BringWindowToTop(hWnd);
        }
    }
}
