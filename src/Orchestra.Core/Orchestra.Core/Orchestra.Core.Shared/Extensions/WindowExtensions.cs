// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using Catel;
    using Catel.Logging;
    
    public static partial class WindowExtensions
    {
        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint SC_CLOSE = 0xF060;
        private const int WM_SHOWWINDOW = 0x00000018;

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
            Argument.IsNotNull(() => window);

            window.SetMaximumWidth();
            window.SetMaximumHeight();
        }

        public static void SetMaximumWidth(this Window window)
        {
            Argument.IsNotNull(() => window);

            window.SetCurrentValue(FrameworkElement.MaxWidthProperty, SystemParameters.WorkArea.Width - 40);
        }

        public static void SetMaximumHeight(this Window window)
        {
            Argument.IsNotNull(() => window);

            window.SetCurrentValue(FrameworkElement.MaxHeightProperty, SystemParameters.WorkArea.Height - 40);
        }

        public static void CenterWindowToScreen(this Window window)
        {
            Argument.IsNotNull(() => window);

            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            window.CenterWindowToSize(new Rect(0, 0, screenWidth, screenHeight));
        }

        public static void CenterWindowToParent(this Window window)
        {
            Argument.IsNotNull(() => window);

            var owner = window.Owner;
            if (owner != null)
            {
                window.CenterWindowToSize(new Rect(owner.Left, owner.Top, owner.ActualWidth, owner.ActualHeight));
                return;
            }

            var parentWindow = window.GetParentWindow();
            if (parentWindow != null)
            {
                window.CenterWindowToSize(new Rect(parentWindow.Left, parentWindow.Top, parentWindow.ActualWidth, parentWindow.ActualHeight));
                return;
            }
        }

        public static void CenterWindowToSize(this Window window, Rect parentRect)
        {
            Argument.IsNotNull(() => window);

            var windowWidth = window.Width;
            var windowHeight = window.Height;

            window.SetCurrentValue(Window.LeftProperty, parentRect.Left + (parentRect.Width / 2) - (windowWidth / 2));
            window.SetCurrentValue(Window.TopProperty, parentRect.Top + (parentRect.Height / 2) - (windowHeight / 2));
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
    }
}