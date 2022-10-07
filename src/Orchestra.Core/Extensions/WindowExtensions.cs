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
    using Orchestra.Win32;
    using Window = System.Windows.Window;
    
    public static partial class WindowExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void DisableCloseButton(this Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            if (!window.IsVisible)
            {
                window.SourceInitialized += OnWindowInitializedForDisableCloseButton;
                return;
            }

            var windowInteropHelper = new WindowInteropHelper(window);

            var sysMenu = User32.GetSystemMenu(windowInteropHelper.Handle, false);
            User32.EnableMenuItem(sysMenu, SC.CLOSE, MF.BYCOMMAND | MF.GRAYED);
        }

        private static void OnWindowInitializedForDisableCloseButton(object? sender, EventArgs e)
        {
            var window = sender as Window;
            if (window is null)
            {
                return;
            }

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
            var foregroundWindowThreadID = User32.GetWindowThreadProcessId(User32.GetForegroundWindow(), IntPtr.Zero);
            var currentThreadID = User32.GetCurrentThreadId();

            if (!User32.AttachThreadInput(foregroundWindowThreadID, currentThreadID, true))
            {
                Log.Warning("Failed to attach to input thread (Win32 code '{0}')", Marshal.GetLastWin32Error());
                return;
            }

            var lastActivePopupWindow = User32.GetLastActivePopup(hWnd);
            User32.SetActiveWindow(lastActivePopupWindow);

            if (!User32.AttachThreadInput(foregroundWindowThreadID, currentThreadID, false))
            {
                Log.Warning("Failed to detach from input thread");
                return;
            }

            User32.BringWindowToTop(hWnd);
        }
    }
}
