namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;

    public class ClosingDetails
    {
        public Window Window { get; set; }
        public Exception Exception { get; set; }
        public bool CanBeClosed { get; set; }
        public bool CanKeepOpened { get; set; }
        public string Message { get; set; }
    }
}
