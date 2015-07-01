// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using Catel;

    public static class WindowExtensions
    {
        #region Fields
        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;
        private const uint SC_CLOSE = 0xF060;
        private const int WM_SHOWWINDOW = 0x00000018;
        #endregion

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

            window.MaxWidth = SystemParameters.WorkArea.Width - 40;
        }

        public static void SetMaximumHeight(this Window window)
        {
            Argument.IsNotNull(() => window);

            window.MaxHeight = SystemParameters.WorkArea.Height - 40;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);
    }
}