namespace Orchestra.Models
{
    using System;
    using MahApps.Metro.Controls;

    public class FlyoutInfo
    {
        public FlyoutInfo(Flyout flyout, object content)
        {
            ArgumentNullException.ThrowIfNull(flyout);
            ArgumentNullException.ThrowIfNull(content);

            Flyout = flyout;
            Content = content;
        }

        public Flyout Flyout { get; set; }
        public object Content { get; set; }
    }
}
