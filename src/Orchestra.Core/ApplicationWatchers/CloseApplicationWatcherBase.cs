// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CloseApplicationWatcherBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;
    using Catel.Threading;

    public abstract class CloseApplicationWatcherBase : ApplicationWatcherBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static bool IsClosingConfirmed;
        private static Window SubscribedWindow;
        private static readonly IList<CloseApplicationWatcherBase> Watchers = new List<CloseApplicationWatcherBase>();
        private static readonly IMessageService MessageService = ServiceLocator.Default.ResolveType<IMessageService>();

        protected CloseApplicationWatcherBase()
        {
            Watchers.Add(this);

            EnqueueShellActivatedAction(Subscribe);
        }

#pragma warning disable AvoidAsyncVoid
        private static async void OnWindowClosing(object sender, CancelEventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
            Log.Debug("Closing main window");

            if (e.Cancel)
            {
                Log.Debug("Closing is cancelled");
                return;
            }

            var window = sender as Window;
            if (window == null)
            {
                Log.Debug("Main window is null");
                return;
            }


            if (!IsClosingConfirmed)
            {
                Log.Debug("Closing is not confirmed yet, perform closing operations first");

                e.Cancel = true;
                await TaskHelper.Run(() => PerformClosingOperationsAsync(window), true);
            }
        }

        private static async Task PerformClosingOperationsAsync(Window window)
        {
            try
            {
                IsClosingConfirmed = await ExecuteClosingAsync(watcher => watcher.PrepareClosingAsync()).ConfigureAwait(false);
                if (!IsClosingConfirmed)
                {
                    return;
                }

                IsClosingConfirmed = await ExecuteClosingAsync(watcher => watcher.ClosingAsync()).ConfigureAwait(false);
                if (IsClosingConfirmed)
                {
                    await CloseWindow(window).ConfigureAwait(false);
                }
                else
                {
                    NotifyClosingCanceled();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to perform closing operations");

                await HandleClosingErrorAsync(window, ex);
            }
        }

        private static async Task HandleClosingErrorAsync(Window window, Exception ex)
        {
            var message = string.IsNullOrEmpty(ex.Message) ? ex.ToString() : ex.Message;

            IsClosingConfirmed = false;

            var closingDetails = new ClosingDetails
            {
                Window = window,
                Exception = ex,
                CanBeClosed = true,
                CanKeepOpened = true,
                Message = $"Error. The application will be forced to close:\n{message}"
            };

            foreach (var watcher in Watchers)
            {
                watcher.ClosingFailed(closingDetails);
            }

            if (string.IsNullOrEmpty(closingDetails.Message) &&
                !closingDetails.CanBeClosed &&
                closingDetails.CanKeepOpened)
            {
                return;
            }

            var messageButton = MessageButton.OKCancel;

            if (!closingDetails.CanKeepOpened)
            {
                messageButton = MessageButton.OK;
            }

            if (await MessageService.ShowAsync(closingDetails.Message, "Error", messageButton, MessageImage.Error) == MessageResult.OK)
            {
                await CloseWindow(window).ConfigureAwait(false);
            }
        }

        private static async Task CloseWindow(Window window)
        {
            IsClosingConfirmed = true;
            await DispatcherService.InvokeAsync(window.Close).ConfigureAwait(false);
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

        protected virtual void ClosingFailed(ClosingDetails appClosingFaultDetails)
        {

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
            if (SubscribedWindow is not null && !SubscribedWindow.Equals(window))
            {
                SubscribedWindow.Closing -= OnWindowClosing;
                SubscribedWindow = null;
            }

            if (SubscribedWindow is null)
            {
                window.Closing += OnWindowClosing;
                SubscribedWindow = window;
            }
        }
    }
}
