// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetPatchHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Windows;
    using Catel.Logging;

    /// <summary>
    /// Class that makes sure to inform the user if a bug occurs due to a missing .net patch.
    /// </summary>
    public static class DotNetPatchHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static bool _isInitialized;

        /// <summary>
        /// Initializes the patch helper.
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        }

        private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!HandleException((Exception) e.ExceptionObject))
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private static bool HandleException(Exception ex)
        {
            LogHelper.AddLogListenerForUnhandledException(ex);

            Log.Info("An unhandled exception occurred, checking if it is a known KB issue: {0}", ex.Message);

            var fileLoadException = ex as FileLoadException;
            if (fileLoadException != null)
            {
                if (fileLoadException.Message.Contains("System, Version=2.0.5.0") ||
                    fileLoadException.Message.Contains("System.Core, Version=2.0.5.0"))
                {
                    // KB2468871
                    const string message = "Unfortunately the application stopped working. It looks like a system update is not installed. Please make sure to update Windows or to download the patch manually from http://support.microsoft.com/kb/2468871";

                    ShowMessage(message);

                    return false;
                }
            }

            LogManager.FlushAll();

            return true;
        }

        private static void ShowMessage(string content)
        {
            Log.Error(content);

            string finalMessage = string.Format("{0}\n\nNote: you can use CTRL + C to copy this message into the clipboard", content);

            MessageBox.Show(finalMessage);
        }
    }
}