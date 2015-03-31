// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FixMaximize.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Windows
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using Catel.MVVM.Tasks;
    using Catel.Windows.Threading;

    /// <summary>
    /// Attachable properties to fix the maximized state of a window.
    /// <para />
    /// The code comes from http://connect.microsoft.com/VisualStudio/feedback/details/775972/wpf-ribbon-window-the-border-is-too-thin.
    /// </summary>
    public class FixMaximize : DependencyObject
    {
        #region FixMaximize

        #region Constants
        public static readonly DependencyProperty FixMaximizeProperty = DependencyProperty.RegisterAttached(
                "FixMaximize", typeof(bool), typeof(FixMaximize), new FrameworkPropertyMetadata(false, FixMaximizeChanged));
        #endregion

        #region Methods
        public static void SetFixMaximize(Window ribbonWindow, bool value)
        {
            ribbonWindow.SetValue(FixMaximizeProperty, value);
        }

        public static bool GetFixMaximize(Window ribbonWindow)
        {
            return (bool)ribbonWindow.GetValue(FixMaximizeProperty);
        }

        private static void FixMaximizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ribbonWindow = (Window)d;
            if ((bool)e.NewValue)
            {
                ribbonWindow.SourceInitialized += FixMaximize_RibbonWindow_SourceInitialized;
            }
        }

        private static void FixMaximize_RibbonWindow_SourceInitialized(object sender, EventArgs e)
        {
            var ribbonWindow = (Window)sender;
            var source = HwndSource.FromHwnd(new WindowInteropHelper(ribbonWindow).Handle);
            source.AddHook(FixMaximize_RibbonWindow_WndProc);
        }

        private static IntPtr FixMaximize_RibbonWindow_WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int AllowedNegativeOffset = 4;

            const int WM_GETMINMAXINFO = 0x0024;
            const uint SWP_NOOWNERZORDER = 0x0200;
            const uint SWP_NOSIZE = 0x0001;
            const int SIZE_MAXIMIZED = 0x2;
            const int WM_SIZE = 0x0005;

            if (msg == WM_SIZE && wParam.ToInt32() == SIZE_MAXIMIZED)
            {
                var rect = new RECT();
                GetWindowRect(hwnd, out rect);

                var newRect = new RECT();

                var xDiff = (rect.left < 0) ? rect.left * -1 : 0;
                var yDiff = (rect.top < 0) ? rect.top * -1 : 0;

                // we need to go from (de)fault (-8,-8) to (-4,-4) we are currently at (1,1)
                newRect.left = rect.left + xDiff - 1 - AllowedNegativeOffset;
                newRect.right = newRect.left + Math.Abs(rect.right - rect.left);
                newRect.top = rect.top + yDiff - 1 - AllowedNegativeOffset;
                newRect.bottom = newRect.top + Math.Abs(rect.bottom - rect.top);

                // Step 1: resize so we have the right size the first time
                int height = newRect.bottom - newRect.top;
                int width = newRect.right - newRect.left;

                var window = (Window)HwndSource.FromHwnd(hwnd).RootVisual;

                var windowSize = GetWindowSize(hwnd);
                height = (height > (int)windowSize.Height) ? (int)windowSize.Height : 0;
                width = (width > (int)windowSize.Width) ? (int)windowSize.Width : 0;
                if (height != 0 || width != 0)
                {
                    height = (int)windowSize.Height;
                    width = (int)windowSize.Width;

                    window.Dispatcher.BeginInvoke(() => SetWindowPos(hwnd, IntPtr.Zero, newRect.left, newRect.top, width, height, SWP_NOOWNERZORDER));
                }

                // Step 2: fix the location, use dispatcher to make sure this code also works the first time an application
                // is started in Maximize state
                window.Dispatcher.BeginInvoke(() => SetWindowPos(hwnd, IntPtr.Zero, newRect.left, newRect.top, 0, 0, SWP_NOOWNERZORDER | SWP_NOSIZE));
            }
            else if (msg == WM_GETMINMAXINFO)
            {
                var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
                //mmi.ptMaxSize.x -= 8; // we are missing 4 pixels left and right
                //mmi.ptMaxSize.y -= 8; // we are missing 4 pixels top and bottom
                mmi.ptMaxPosition.x = 1; // need to set this to positive value for MaxSize to have any effect, we will reposition later anyway
                mmi.ptMaxPosition.y = 1; // need to set this to positive value for MaxSize to have any effect, we will reposition later anyway

                var windowSize = GetWindowSize(hwnd);
                mmi.ptMaxSize.x = (int)windowSize.Width;
                mmi.ptMaxSize.y = (int)windowSize.Height;
                mmi.ptMinTrackSize.x = mmi.ptMaxSize.x;
                mmi.ptMinTrackSize.x = mmi.ptMaxSize.y;

                Marshal.StructureToPtr(mmi, lParam, false);
            }

            return IntPtr.Zero;
        }

        private static Size GetWindowSize(IntPtr hWnd)
        {
            var window = (Window)HwndSource.FromHwnd(hWnd).RootVisual;
            var screen = WpfScreen.GetScreenFrom(window);

            var size = new Size(screen.WorkingArea.Width + 8, screen.WorkingArea.Height + 8);
            return size;
        }

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        #endregion

        #region Nested type: MINMAXINFO
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };
        #endregion

        #region Nested type: POINT
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }
        #endregion

        #region Nested type: RECT
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        #endregion

        #endregion
    }
}