// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RichTextBoxLogListener.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel;
    using Catel.Logging;
    using Catel.Windows.Threading;

    public class RichTextBoxLogListener : LogListenerBase
    {
        #region Constants
        private static readonly Dictionary<LogEvent, Brush> ColorSets = new Dictionary<LogEvent, Brush>();
        #endregion

        #region Fields
        private readonly RichTextBox _textBox;
        #endregion

        #region Constructors
        static RichTextBoxLogListener()
        {
            ColorSets[LogEvent.Debug] = Brushes.Gray;
            ColorSets[LogEvent.Info] = Brushes.Black;
            ColorSets[LogEvent.Warning] = Brushes.DarkOrange;
            ColorSets[LogEvent.Error] = Brushes.Red;
        }

        public RichTextBoxLogListener(RichTextBox richTextBox)
        {
            ArgumentNullException.ThrowIfNull(richTextBox);

            _textBox = richTextBox;

            Clear();
        }
        #endregion

        #region Methods
        public void Clear()
        {
            _textBox.Dispatcher.Invoke(() => _textBox.Document.Blocks.Clear());
        }

        protected override void Write(ILog log, string message, LogEvent logEvent, object extraData, LogData logData, DateTime time)
        {
            _textBox.Dispatcher.Invoke(() =>
            {
                var finalMessage = string.Format("{0} {1}", time.ToString("HH:mm:ss.fff"), message);

                var paragraph = new Paragraph(new Run(finalMessage));

                FixParagraphSpacing(paragraph);

                paragraph.Foreground = ColorSets[logEvent];
                _textBox.Document.Blocks.Add(paragraph);
                _textBox.ScrollToEnd();
            });
        }

        private void FixParagraphSpacing(Paragraph paragraph)
        {
            paragraph.SetCurrentValue(Block.MarginProperty, new Thickness(0));
        }
        #endregion
    }
}