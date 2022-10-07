namespace Orchestra
{
    using System;
    using System.Windows;

    public class ClosingDetails
    {
        public ClosingDetails(Window window)
        {
            ArgumentNullException.ThrowIfNull(window);

            Window = window;
            Message = string.Empty;
        }

        public Window Window { get; private set; }
        public Exception? Exception { get; set; }
        public bool CanBeClosed { get; set; }
        public bool CanKeepOpened { get; set; }
        public string Message { get; set; }
    }
}
