// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxLogListener.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Logging
{
    using System;
    using System.Windows.Controls;
    using Catel;
    using Catel.Logging;
    using Catel.Windows.Threading;

    public class TextBoxLogListener : LogListenerBase
    {
        #region Fields
        private readonly TextBox _textBox;
        #endregion

        #region Constructors
        public TextBoxLogListener(TextBox textBox)
        {
            Argument.IsNotNull(() => textBox);

            _textBox = textBox;
        }
        #endregion

        #region Methods
        public void Clear()
        {
            _textBox.Dispatcher.Invoke(() => _textBox.Clear());
        }

        protected override void Write(ILog log, string message, LogEvent logEvent, object extraData, LogData logData, DateTime time)
        {
            _textBox.Dispatcher.Invoke(() =>
            {
                _textBox.AppendText(string.Format("{0} {1}", time.ToString("HH:mm:ss.fff"), message));
                _textBox.AppendText(Environment.NewLine);
                _textBox.ScrollToEnd();
            });
        }
        #endregion
    }
}