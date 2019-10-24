// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressPleaseWaitService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;

    internal class ProgressPleaseWaitService : PleaseWaitService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDependencyResolver _dependencyResolver;
        private ProgressBar _progressBar;
        private ResourceDictionary _resourceDictionary;

        private readonly DispatcherTimer _hidingTimer;

        public ProgressPleaseWaitService(IDispatcherService dispatcherService, IDependencyResolver dependencyResolver)
            : base(dispatcherService)
        {
            Argument.IsNotNull(() => dependencyResolver);

            _dependencyResolver = dependencyResolver;

            _hidingTimer = new DispatcherTimer
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
            if (progressBar != null)
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
                        Log.Debug("Showing progress bar");

                        _hidingTimer.Stop();

                        progressBar.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Visible);
                    }                    
                }, true);
            }
        }

        private void OnHideTimerTick(object sender, EventArgs eventArgs)
        {
            Log.Debug("Hiding progress bar");

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

        private ProgressBar InitializeProgressBar()
        {
            if (_progressBar is null)
            {
                _progressBar = _dependencyResolver.TryResolve<ProgressBar>("pleaseWaitService");

                if (_progressBar != null)
                {
                    Log.Debug("Found progress bar that will represent progress inside the ProgressPleaseWaitService");
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
