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
        private const string SizeSeparator = "x";

        public static void SaveWindowSize(this Window window)
        {
            Argument.IsNotNull(() => window);

            var windowName = window.GetType().Name;

            Log.Debug($"Saving window size for '{windowName}'");

            var storageFile = GetWindowStorageFile(window);

            try
            {
                var culture = CultureInfo.InvariantCulture;

                File.WriteAllText(storageFile, $"{ObjectToStringHelper.ToString(window.Width, culture)}{SizeSeparator}{ObjectToStringHelper.ToString(window.Height, culture)}");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to save window size to file '{storageFile}'");
            }
        }

        public static void LoadWindowSize(this Window window)
        {
            Argument.IsNotNull(() => window);

            var windowName = window.GetType().Name;

            Log.Debug($"Loading window size for '{windowName}'");

            var storageFile = GetWindowStorageFile(window);
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

                var splitted = sizeText.Split(new [] { SizeSeparator }, StringSplitOptions.RemoveEmptyEntries);
                if (splitted.Length != 2)
                {
                    Log.Warning($"Size text for window could not be splitted correctly, cannot restore window size");
                    return;
                }

                var culture = CultureInfo.InvariantCulture;

                var width = StringToObjectHelper.ToDouble(splitted[0], culture);
                var height = StringToObjectHelper.ToDouble(splitted[1], culture);

                Log.Debug($"Setting window size for '{windowName}' to '{width} x {height}'");

                window.Width = width;
                window.Height = height;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, $"Failed to load window size from file '{storageFile}'");
            }
        }

        private static string GetWindowStorageFile(this Window window)
        {
            Argument.IsNotNull(() => window);

            var appData = Path.GetApplicationDataDirectory();
            var directory = Path.Combine(appData, "windows");

            Directory.CreateDirectory(directory);

            var file = Path.Combine(directory, $"{window.GetType().FullName}.dat");
            return file;
        }
    }
}