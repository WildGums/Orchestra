// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PleaseWaitService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Windows.Input;
    using Catel;
    using Catel.Logging;
    using Catel.Services;

    public class PleaseWaitService : IPleaseWaitService
    {
        #region Constants
        public static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Fields
        private readonly IDispatcherService _dispatcherService;

        private int _counter;
        private Cursor _previousCursor;
        #endregion

        #region Constructors
        public PleaseWaitService(IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => dispatcherService);

            _dispatcherService = dispatcherService;
        }
        #endregion

        #region IPleaseWaitService Members
        public void Show(string status = "")
        {
            UpdateStatus(status);

            _dispatcherService.BeginInvokeIfRequired(() =>
            {
                if (_previousCursor == null)
                {
                    _previousCursor = Mouse.OverrideCursor;
                }

                Mouse.OverrideCursor = Cursors.Wait;
            });
        }

        public void Show(PleaseWaitWorkDelegate workDelegate, string status = "")
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

        public void UpdateStatus(string status)
        {
            if (!string.IsNullOrWhiteSpace(status))
            {
                Log.Info(status);
            }
        }

        public void UpdateStatus(int currentItem, int totalItems, string statusFormat = "")
        {
            // not required
        }

        public void Hide()
        {
            _dispatcherService.BeginInvokeIfRequired(() =>
            {
                Mouse.OverrideCursor = _previousCursor;

                _previousCursor = null;
                _counter = 0;
            });
        }

        public void Push(string status = "")
        {
            if (_counter == 0)
            {
                Show(status);
            }

            _counter++;
        }

        public void Pop()
        {
            _counter--;

            if (_counter <= 0)
            {
                Hide();
            }
        }
        #endregion
    }
}