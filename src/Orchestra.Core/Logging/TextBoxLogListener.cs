namespace Orchestra.Logging
{
    using System;
    using System.Windows.Controls;
    using Catel.Logging;

    public class TextBoxLogListener : LogListenerBase
    {

        private readonly TextBox _textBox;

        public TextBoxLogListener(TextBox textBox)
        {
            ArgumentNullException.ThrowIfNull(textBox);

            _textBox = textBox;
        }

        public void Clear()
        {
            _textBox.Dispatcher.Invoke(() => _textBox.Clear());
        }

        protected override void Write(ILog log, string message, LogEvent logEvent, object? extraData, LogData? logData, DateTime time)
        {
            _textBox.Dispatcher.Invoke(() =>
            {
                _textBox.AppendText(string.Format("{0} {1}", time.ToString("HH:mm:ss.fff"), message));
                _textBox.AppendText(Environment.NewLine);
                _textBox.ScrollToEnd();
            });
        }
    }
}
