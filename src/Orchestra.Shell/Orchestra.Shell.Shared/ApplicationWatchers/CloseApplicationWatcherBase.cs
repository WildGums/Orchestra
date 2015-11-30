// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseApplicationWatcherBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel.Services;
    using Catel.Threading;

    public abstract class CloseApplicationWatcherBase : ApplicationWatcherBase
    {
        private static bool _isClosingConfirmed;
        private static readonly IList<CloseApplicationWatcherBase> Watchers = new List<CloseApplicationWatcherBase>();

        protected CloseApplicationWatcherBase()
        {
            Watchers.Add(this);

            EnqueueShellActivatedAction(Subscribe);
        }

        private static async void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
            {
                return;
            }

            var window = sender as Window;
            if (window == null)
            {
                return;
            }

            if (!_isClosingConfirmed)
            {
                e.Cancel = true;
                await TaskHelper.Run(() => PerformClosingOperationsAsync(window), true);
            }
        }

        private static async Task PerformClosingOperationsAsync(Window window)
        {
            _isClosingConfirmed = await ExecuteClosingAsync(watcher => watcher.PrepareClosingAsync()).ConfigureAwait(false);
            if (!_isClosingConfirmed)
            {
                return;
            }

            _isClosingConfirmed = await ExecuteClosingAsync(watcher => watcher.ClosingAsync()).ConfigureAwait(false);
            if (_isClosingConfirmed)
            {
                await DispatcherService.InvokeAsync(window.Close).ConfigureAwait(false);
            }
            else
            {
                NotifyClosingCanceled();
            }
        }

        private static void NotifyClosingCanceled()
        {
            foreach (var watcher in Watchers)
            {
                watcher.ClosingCanceled();
            }
        }

        private static async Task<bool> ExecuteClosingAsync(Func<CloseApplicationWatcherBase, Task<bool>> operation)
        {
            foreach (var watcher in Watchers)
            {
                if (!await operation(watcher).ConfigureAwait(false))
                {
                    NotifyClosingCanceled();

                    return false;
                }
            }

            return true;
        }

        protected virtual void ClosingCanceled()
        {
            
        }

        protected virtual Task<bool> PrepareClosingAsync()
        {
            return TaskHelper<bool>.FromResult(true);
        }

        protected virtual Task<bool> ClosingAsync()
        {
            return TaskHelper<bool>.FromResult(true);
        }

        private static void Subscribe(Window window)
        {
            window.Closing += OnWindowClosing;
        }
    }
}