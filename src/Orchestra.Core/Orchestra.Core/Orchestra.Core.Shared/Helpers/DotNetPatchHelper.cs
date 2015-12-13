// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetPatchHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.ExceptionServices;
    using System.Windows;
    using System.Windows.Threading;
    using Catel.Logging;

    /// <summary>
    /// Class that makes sure to inform the user if a bug occurs due to a missing .net patch.
    /// </summary>
    public static class DotNetPatchHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly Queue<Exception> LastFirstChanceExceptions = new Queue<Exception>();

        private static bool _isAppDomainInitialized;
        private static bool _isApplicationInitialized;
        private static bool _handledException;

        /// <summary>
        /// Initializes the patch helper.
        /// </summary>
        public static void Initialize()
        {
            InitializeAppDomain();
            InitializeApplication();
        }

        private static void InitializeAppDomain()
        {
            if (_isAppDomainInitialized)
            {
                return;
            }

            _isAppDomainInitialized = true;

            var appDomain = AppDomain.CurrentDomain;
            appDomain.UnhandledException += OnAppDomainUnhandledException;
            appDomain.FirstChanceException += OnAppDomainFirstChanceException;
        }

        private static void InitializeApplication()
        {
            if (_isApplicationInitialized)
            {
                return;
            }

            var application = Application.Current;
            if (application != null)
            {
                application.DispatcherUnhandledException += OnDispatcherUnhandledException;
                _isApplicationInitialized = true;
            }
        }

        private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Debug("AppDomain unhandled exception");

            if (_handledException)
            {
                Log.Debug("Already handled an unhandled exception");
                return;
            }

            if (!HandleException((Exception)e.ExceptionObject))
            {
                _handledException = true;
                Process.GetCurrentProcess().Kill();
            }
        }

        private static void OnAppDomainFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            lock (LastFirstChanceExceptions)
            {
                LastFirstChanceExceptions.Enqueue(e.Exception);

                while (LastFirstChanceExceptions.Count > 5)
                {
                    LastFirstChanceExceptions.Dequeue();
                }
            }
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Debug("Dispatcher unhandled exception");

            if (_handledException)
            {
                Log.Debug("Already handled an unhandled exception");
                return;
            }

            if (!HandleException(e.Exception))
            {
                _handledException = true;
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

                    LogManager.FlushAll();

                    return false;
                }
            }

            Log.Info("Below is a list of the last first chance exceptions that occurred. It might provide more information to the issue.");

            lock (LastFirstChanceExceptions)
            {
                while (LastFirstChanceExceptions.Count > 0)
                {
                    var firstChanceException = LastFirstChanceExceptions.Dequeue();
                    
                    Log.Info("================================================================================================");
                    Log.Info();
                    Log.Info(firstChanceException);
                    Log.Info();
                }
            }

            LogManager.FlushAll();

            return true;
        }

        private static void ShowMessage(string content)
        {
            Log.Error(content);

            var finalMessage = string.Format("{0}\n\nNote: you can use CTRL + C to copy this message into the clipboard", content);

            MessageBox.Show(finalMessage);
        }
    }
}