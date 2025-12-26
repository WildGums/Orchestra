namespace Orchestra
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    using Catel.Services;
    using Catel.Windows.Threading;
    using Microsoft.Extensions.Logging;

    internal class ProgressBusyIndicatorService : BusyIndicatorService
    {
        private ProgressBar? _progressBar;
        private ResourceDictionary? _resourceDictionary;

        private readonly DispatcherTimerEx _hidingTimer;
        private readonly IProgressBarProvider _progressBarProvider;

        public ProgressBusyIndicatorService(ILogger<BusyIndicatorService> logger, 
            IDispatcherService dispatcherService, IProgressBarProvider progressBarProvider)
            : base(logger, dispatcherService)
        {
            _progressBarProvider = progressBarProvider;

            _hidingTimer = new DispatcherTimerEx(dispatcherService)
            {
                Interval = TimeSpan.FromMilliseconds(10)
            };

            _hidingTimer.Tick += OnHideTimerTick;
        }

        public override void Hide()
        {
            base.Hide();

            _hidingTimer.Start();
        }

        public override void UpdateStatus(int currentItem, int totalItems, string statusFormat = "")
        {
            base.UpdateStatus(currentItem, totalItems, statusFormat);

            var progressBar = InitializeProgressBar();
            if (progressBar is not null)
            {
                _dispatcherService.BeginInvoke(() =>
                {
                    progressBar.SetCurrentValue(System.Windows.Controls.Primitives.RangeBase.MinimumProperty, (double)0);
                    progressBar.SetCurrentValue(System.Windows.Controls.Primitives.RangeBase.MaximumProperty, (double)totalItems);
                    progressBar.SetCurrentValue(System.Windows.Controls.Primitives.RangeBase.ValueProperty, (double)currentItem);

                    if (currentItem < 0 || currentItem >= totalItems)
                    {
                        Hide();
                    }
                    else if (progressBar.Visibility != Visibility.Visible)
                    {
                        _logger.LogDebug("Showing progress bar");

                        _hidingTimer.Stop();

                        progressBar.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Visible);
                    }                    
                }, true);
            }
        }

        private void OnHideTimerTick(object? sender, EventArgs eventArgs)
        {
            _logger.LogDebug("Hiding progress bar");

            _hidingTimer.Stop();

            var progressBar = InitializeProgressBar();
            if (progressBar is null)
            {
                return;
            }

            var storyboard = GetHideProgressBarStoryboard();

            storyboard.Completed += (s, e) =>
            {
                progressBar.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
            };

            storyboard.Begin(progressBar);
        }

        private ProgressBar? InitializeProgressBar()
        {
            if (_progressBar is null)
            {
                _progressBar = _progressBarProvider.GetProgressBar();

                if (_progressBar is not null)
                {
                    _logger.LogDebug("Found progress bar that will represent progress inside the ProgressBusyIndicatorService");
                }
            }

            return _progressBar;
        }

        private Storyboard GetHideProgressBarStoryboard()
        {
            if (_resourceDictionary is null)
            {
                _resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("/Orchestra.Core;Component/Themes/Generic.xaml", UriKind.RelativeOrAbsolute)
                };
            }

            var storyBoard = (Storyboard)_resourceDictionary["FadeOutStoryboard"];
            return storyBoard;
        }
    }
}
