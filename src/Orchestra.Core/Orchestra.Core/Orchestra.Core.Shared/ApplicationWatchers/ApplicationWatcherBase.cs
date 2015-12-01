// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationWatcherBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Catel.IoC;
    using Catel.Services;

    public abstract class ApplicationWatcherBase
    {
        protected static readonly IDispatcherService DispatcherService;

        private static Window _mainWindow;
        private static readonly DispatcherTimer DispatcherTimer;
        private static Queue<Action<Window>> _shellActivatedActions;
        private static readonly object _lock = new object();

        static ApplicationWatcherBase()
        {
            EnsureSubscribesInitialized();

            DispatcherService = ServiceLocator.Default.ResolveType<IDispatcherService>();

            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = TimeSpan.FromMilliseconds(5);
            DispatcherTimer.Tick += (sender, e) => EnsureMainWindow();
            DispatcherTimer.Start();

            EnsureMainWindow();
        }

        protected void EnqueueShellActivatedAction(Action<Window> action)
        {
            EnsureSubscribesInitialized();

            lock (_lock)
            {
                _shellActivatedActions.Enqueue(action);
            }

            DispatcherTimer.Start();
        }

        private static void EnsureSubscribesInitialized()
        {
            if (_shellActivatedActions == null)
            {
                lock (_lock)
                {
                    _shellActivatedActions = new Queue<Action<Window>>();
                }
            }
        }

        private static void EnsureMainWindow()
        {
            DispatcherTimer.Stop();

            _mainWindow = System.Windows.Application.Current.MainWindow;
            if (_mainWindow is Orchestra.Views.SplashScreen)
            {
                _mainWindow = null;
            }

            if (_mainWindow == null)
            {
                DispatcherTimer.Start();
                return;
            }

            if (_shellActivatedActions == null)
            {
                DispatcherTimer.Start();
                return;
            }

            lock (_lock)
            {
                while (_shellActivatedActions.Any())
                {
                    var action = _shellActivatedActions.Dequeue();
                    action(_mainWindow);
                }
            }
        }
    }
}