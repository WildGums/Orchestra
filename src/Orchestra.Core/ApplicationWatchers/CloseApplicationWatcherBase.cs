namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;

    public abstract class CloseApplicationWatcherBase : ApplicationWatcherBase
    {
        private static bool CanClose = false;
        private static bool IsHandlingClosing = false;

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

        internal static bool SkipClosing { get; set; }

        /// <summary>
        /// Only used to reset the state for unit tests.
        /// </summary>
        protected internal static void Reset()
        {
            var lastWatcher = Watchers.LastOrDefault();
            Watchers.Clear();

            if (lastWatcher is not null)
            {
                Watchers.Add(lastWatcher);
            }

            IsClosingConfirmed = false;
            IsHandlingClosing = false;
            CanClose = false;
        }

#pragma warning disable AvoidAsyncVoid
        private static async void OnWindowClosing(object sender, CancelEventArgs e)
#pragma warning restore AvoidAsyncVoid
        {
            if (CanClose)
            {
                e.Cancel = false;
                return;
            }

            // Step 1: always cancel the closing, we will take over from here
            e.Cancel = true;

            if (IsClosingConfirmed)
            {
                // We need to run the closing methods, so still need to cancel
                return;
            }

            if (IsHandlingClosing)
            {
                // Already handling
                return;
            }

            if (sender is not Window window)
            {
                Log.Debug("Main window is null");
                return;
            }

            if (!SkipClosing)
            {
                try
                {
                    IsHandlingClosing = true;

                    Log.Debug("Closing main window");

                    if (!IsClosingConfirmed)
                    {
                        Log.Debug("Closing is not confirmed yet, perform closing operations first");

                        await Task.Run(() => PerformClosingOperationsAsync(window));
                    }
                }
                finally
                {
                    IsHandlingClosing = false;
                }
            }

            if (!IsClosingConfirmed)
            {
                Log.Debug("At least 1 watcher requested canceling the closing of the window");
                return;
            }

            Log.Debug("Perform closed operations after closing confirmed");

            await PerformClosedOperationsAsync();

            // Fully done, now really close
            CanClose = true;

            // Close window (again, we will not interfere this time)
            await CloseWindowAsync(window).ConfigureAwait(false);
        }

        private static async Task PerformClosingOperationsAsync(Window window)
        {
            try
            {
                Log.Debug("Prepare closing operations");

                IsClosingConfirmed = await ExecuteClosingAsync(PrepareClosingAsync).ConfigureAwait(false);
                if (!IsClosingConfirmed)
                {
                    Log.Debug("Closing is not confirmed, canceling closing operations");
                    return;
                }

                Log.Debug("Performing closing operations");

                IsClosingConfirmed = await ExecuteClosingAsync(ClosingAsync).ConfigureAwait(false);
                if (IsClosingConfirmed)
                {
                    Log.Debug("Closing confirmed, continue execution");
                }
                else
                {
                    Log.Debug("Closing cancelled, raising notification");

                    NotifyClosingCanceled();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignore, don't log
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to perform closing operations");

                await HandleClosingErrorAsync(window, ex);
            }
        }

        internal static async Task PerformClosedOperationsAsync()
        {
            await ExecuteClosedAsync(ClosedAsync);
        }

        private static async Task<bool> PrepareClosingAsync(CloseApplicationWatcherBase watcher)
        {
            try
            {
                Log.Debug($"Executing PrepareClosingAsync() for '{ObjectToStringHelper.ToFullTypeString(watcher)}'");

                var result = await watcher.PrepareClosingAsync();

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to execute PrepareClosingAsync() for '{ObjectToStringHelper.ToFullTypeString(watcher)}'. Continue to run all watchers left.");
                return true;
            }
        }

        private static async Task<bool> ClosingAsync(CloseApplicationWatcherBase watcher)
        {
            try
            {
                Log.Debug($"Executing ClosingAsync() for '{ObjectToStringHelper.ToFullTypeString(watcher)}'");

                var result = await watcher.ClosingAsync();

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to execute ClosingAsync() for '{ObjectToStringHelper.ToFullTypeString(watcher)}'. Continue to run all watchers left.");
                return true;
            }
        }

        private static async Task ClosedAsync(CloseApplicationWatcherBase watcher)
        {
            try
            {
                Log.Debug($"Executing ClosedAsync() for '{ObjectToStringHelper.ToFullTypeString(watcher)}'");
                await watcher.ClosedAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to execute ClosedAsync() for '{ObjectToStringHelper.ToFullTypeString(watcher)}'. Continue to run all watchers left.");
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
                await CloseWindowAsync(window).ConfigureAwait(false);
            }
        }

        private static async Task CloseWindowAsync(Window window)
        {
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
            Log.Debug($"Execute operation for each of {Watchers.Count} watcher");

            foreach (var watcher in Watchers)
            {
                if (!await operation(watcher).ConfigureAwait(false))
                {
                    return false;
                }
            }

            return true;
        }

        private static async Task ExecuteClosedAsync(Func<CloseApplicationWatcherBase, Task> operation)
        {
            Log.Debug($"Execute operation for each of {Watchers.Count} watcher");

            foreach (var watcher in Watchers)
            {
                await operation(watcher);
            }
        }

        protected virtual void ClosingFailed(ClosingDetails appClosingFaultDetails)
        {

        }

        protected virtual void ClosingCanceled()
        {

        }

        protected virtual Task<bool> PrepareClosingAsync()
        {
            return Task<bool>.FromResult(true);
        }

        protected virtual Task<bool> ClosingAsync()
        {
            return Task<bool>.FromResult(true);
        }

        protected virtual Task ClosedAsync()
        {
            return Task.CompletedTask;
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
