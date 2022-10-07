namespace Orchestra.Services
{
    using System;
    using System.Windows.Input;
    using Catel.Logging;
    using Catel.Services;

    public class BusyIndicatorService : IBusyIndicatorService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected readonly IDispatcherService _dispatcherService;

        private Cursor? _previousCursor;

        public BusyIndicatorService(IDispatcherService dispatcherService)
        {
            ArgumentNullException.ThrowIfNull(dispatcherService);

            _dispatcherService = dispatcherService;
        }

        public int ShowCounter { get; private set; }

        /// <summary>
        /// Gets the last set current item.
        /// </summary>
        protected int CurrentItem { get; private set; }

        /// <summary>
        /// Gets the last set total items.
        /// </summary>
        protected int TotalItems { get; private set; }

        /// <summary>
        /// Gets whether the <see cref="CurrentItem"/> equals the <see cref="TotalItems"/>.
        /// </summary>
        protected bool ReachedTotalItems
        {
            get
            {
                if (CurrentItem < 0)
                {
                    return true;
                }

                return CurrentItem >= TotalItems;
            }
        }

        public virtual void Show(string status = "")
        {
            Log.Debug("Showing busy indicator");

            if (ShowCounter <= 0)
            {
                ShowCounter = 1;
            }

            UpdateStatus(status);

            _dispatcherService.BeginInvokeIfRequired(() =>
            {
                if (_previousCursor is null)
                {
                    _previousCursor = Mouse.OverrideCursor;
                }

                Mouse.OverrideCursor = Cursors.Wait;
            });
        }

        public virtual void Show(BusyIndicatorWorkDelegate workDelegate, string status = "")
        {
            Show(status);

            try
            {
                workDelegate();
            }
            finally
            {
                Hide();
            }
        }

        public virtual async void Show(BusyIndicatorWorkAsyncDelegate workDelegate, string status = "")
        {
            Show(status);

            try
            {
                await workDelegate();
            }
            finally
            {
                Hide();
            }
        }

        public virtual void UpdateStatus(string status)
        {
            if (!string.IsNullOrWhiteSpace(status))
            {
                Log.Info(status);
            }
        }

        public virtual void UpdateStatus(int currentItem, int totalItems, string statusFormat = "")
        {
            CurrentItem = currentItem;
            TotalItems = totalItems;

            if (currentItem >= 0 && ShowCounter <= 0)
            {
                Show();
            }

            // If we reached the end, count 1 down
            if (currentItem > 0 && ReachedTotalItems)
            {
                ShowCounter--;
            }

            HideIfRequired();
        }

        public virtual void Hide()
        {
            Log.Debug("Hiding busy indicator");

            CurrentItem = -1;
            ShowCounter = 0;

            _dispatcherService.BeginInvokeIfRequired(() =>
            {
                Mouse.OverrideCursor = _previousCursor;

                _previousCursor = null;
            });
        }

        public virtual void Push(string status = "")
        {
            if (ShowCounter == 0)
            {
                Show(status);
            }
            else
            {
                ShowCounter++;
            }

            Log.Debug($"Pushed busy indicator, counter is '{ShowCounter}'");
        }

        public virtual void Pop()
        {
            ShowCounter--;

            Log.Debug($"Popped busy indicator, counter is '{ShowCounter}'");

            HideIfRequired();
        }

        protected virtual void HideIfRequired()
        {
            // If we are popping, we should respect the total items (of the progress) as well
            if (ShowCounter <= 0 && ReachedTotalItems)
            {
                Hide();
            }
        }
    }
}
