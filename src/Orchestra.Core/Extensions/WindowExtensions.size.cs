// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowExtensions.size.cs" company="WildGums">
//   Copyright (c) 2008 - 2016 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using Path = Catel.IO.Path;

    public static partial class WindowExtensions
    {
        private const string SizeSeparator = "|";

        [ObsoleteEx(ReplacementTypeOrMember = "Orc.Controls.WindowExtensions.SaveWindowSize", TreatAsErrorFromVersion = "6.0", RemoveInVersion = "7.0")]
        public static void SaveWindowSize(this Window window)
        {
            SaveWindowSize(window, null);
        }

        [ObsoleteEx(ReplacementTypeOrMember = "Orc.Controls.WindowExtensions.SaveWindowSize", TreatAsErrorFromVersion = "6.0", RemoveInVersion = "7.0")]
        public static void SaveWindowSize(this Window window, string tag)
        {
            Argument.IsNotNull(() => window);

            var windowName = window.GetType().Name;

            Log.Debug($"Saving window size for '{windowName}'");

            var storageFile = GetWindowStorageFile(window, tag);

            try
            {
                var culture = CultureInfo.InvariantCulture;

                var width = ObjectToStringHelper.ToString(window.Width, culture);
                var height = ObjectToStringHelper.ToString(window.Height, culture);
                var left = ObjectToStringHelper.ToString(window.Left, culture);
                var top = ObjectToStringHelper.ToString(window.Top, culture);
                var state = ObjectToStringHelper.ToString(window.WindowState, culture);

                var contents = $"{width}{SizeSeparator}{height}{SizeSeparator}{state}{SizeSeparator}{left}{SizeSeparator}{top}";

                File.WriteAllText(storageFile, contents);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to save window size to file '{storageFile}'");
            }
        }

        [ObsoleteEx(ReplacementTypeOrMember = "Orc.Controls.WindowExtensions.LoadWindowSize", TreatAsErrorFromVersion = "6.0", RemoveInVersion = "7.0")]
        public static void LoadWindowSize(this Window window, bool restoreWindowState)
        {
            LoadWindowSize(window, null, restoreWindowState, true);
        }

        [ObsoleteEx(ReplacementTypeOrMember = "Orc.Controls.WindowExtensions.LoadWindowSize", TreatAsErrorFromVersion = "6.0", RemoveInVersion = "7.0")]
        public static void LoadWindowSize(this Window window, string tag = null, bool restoreWindowState = false, bool restoreWindowPosition = true)
        {
            Argument.IsNotNull(() => window);

            var windowName = window.GetType().Name;

            Log.Debug($"Loading window size for '{windowName}'");

            var storageFile = GetWindowStorageFile(window, tag);
            if (!File.Exists(storageFile))
            {
                Log.Debug($"Window size file '{storageFile}' does not exist, cannot restore window size");
                return;
            }

            try
            {
                var sizeText = File.ReadAllText(storageFile);
                if (string.IsNullOrWhiteSpace(sizeText))
                {
                    Log.Warning($"Size text for window is empty, cannot restore window size");
                    return;
                }

                var splitted = sizeText.Split(new[] { SizeSeparator }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted.Length < 2)
                {
                    Log.Warning($"Size text for window could not be splitted correctly, cannot restore window size");
                    return;
                }

                var culture = CultureInfo.InvariantCulture;

                var width = StringToObjectHelper.ToDouble(splitted[0], culture);
                var height = StringToObjectHelper.ToDouble(splitted[1], culture);

                Log.Debug($"Setting window size for '{windowName}' to '{width} x {height}'");

                window.SetCurrentValue(FrameworkElement.WidthProperty, width);
                window.SetCurrentValue(FrameworkElement.HeightProperty, height);

                if (restoreWindowState && splitted.Length > 2)
                {
                    var windowState = Enum<WindowState>.Parse(splitted[2]);
                    if (windowState != window.WindowState)
                    {
                        Log.Debug($"Restoring window state for '{windowName}' to '{windowState}'");

                        window.SetCurrentValue(Window.WindowStateProperty, windowState);
                    }
                }

                if (restoreWindowPosition && splitted.Length > 3)
                {
                    var left = StringToObjectHelper.ToDouble(splitted[3], culture);
                    var top = StringToObjectHelper.ToDouble(splitted[4], culture);

                    Log.Debug($"Restoring window position for '{windowName}' to '{left} (x) / {top} (y)'");

                    var virtualScreenLeft = SystemParameters.VirtualScreenLeft;
                    var virtualScreenTop = SystemParameters.VirtualScreenTop;
                    var virtualScreenWidth = SystemParameters.VirtualScreenWidth;
                    var virtualScreenHeight = SystemParameters.VirtualScreenHeight;

                    if (left < virtualScreenLeft ||
                        left + width > virtualScreenWidth ||
                        top < virtualScreenTop ||
                        top + height > virtualScreenHeight)
                    {
                        window.CenterWindowToParent();
                        return;
                    }

                    window.SetCurrentValue(Window.LeftProperty, left);
                    window.SetCurrentValue(Window.TopProperty, top);
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to load window size from file '{storageFile}'");
            }
        }

        private static string GetWindowStorageFile(this Window window, string tag)
        {
            Argument.IsNotNull(() => window);

            var appData = Catel.IO.Path.GetApplicationDataDirectory();
            var directory = Path.Combine(appData, "windows");

            Directory.CreateDirectory(directory);

            var tagToUse = string.Empty;
            if (!string.IsNullOrWhiteSpace(tag))
            {
                tagToUse = $"_{tag.GetSlug()}";
            }

            var file = Path.Combine(directory, $"{window.GetType().FullName}{tagToUse}.dat");
            return file;
        }
    }
}
