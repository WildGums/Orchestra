// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressPleaseWaitService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Services;

    internal class ProgressPleaseWaitService: PleaseWaitService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDependencyResolver _dependencyResolver;
        private ProgressBar _progressBar;

        public ProgressPleaseWaitService(IDispatcherService dispatcherService, IDependencyResolver dependencyResolver) 
            : base(dispatcherService)
        {
            Argument.IsNotNull(()=> dependencyResolver);

            _dependencyResolver = dependencyResolver;
        }

        public override void Hide()
        {
            base.Hide();

            var progressBar = InitializeProgressBar();
            if (progressBar != null)
            {
                _dispatcherService.BeginInvoke(() =>
                {
                    Log.Debug("Hiding progress bar");

                    progressBar.Visibility = Visibility.Collapsed;
                });
            }
        }

        public override void UpdateStatus(int currentItem, int totalItems, string statusFormat = "")
        {
            base.UpdateStatus(currentItem, totalItems, statusFormat);

            var progressBar = InitializeProgressBar();
            if (progressBar != null)
            {
                _dispatcherService.BeginInvoke(() =>
                {
                    progressBar.Minimum = 0;
                    progressBar.Maximum = totalItems;
                    progressBar.Value = currentItem;

                    if (currentItem < 0 || currentItem >= totalItems)
                    {
                        Log.Debug("Hiding progress bar");

                        progressBar.Visibility = Visibility.Collapsed;
                    }
                    else if (progressBar.Visibility != Visibility.Visible)
                    {
                        Log.Debug("Showing progress bar");

                        progressBar.Visibility = Visibility.Visible;
                    }
                });
            }
        }

        private ProgressBar InitializeProgressBar()
        {
            if (_progressBar == null)
            {
                _progressBar = _dependencyResolver.TryResolve<ProgressBar>("pleaseWaitService");

                if (_progressBar != null)
                {
                    Log.Debug("Found progress bar that will represent progress inside the ProgressPleaseWaitService");
                }
            }

            return _progressBar;
        }
    }
}