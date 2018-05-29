namespace Orchestra
{
    using System.Windows;

    internal static class ShellDimensionsHelper
    {
        public static void ApplyDimensions(Window window, FrameworkElement mainView)
        {
            var minHeight = mainView.MinHeight;
            if (!double.IsNaN(minHeight))
            {
                window.SetCurrentValue(Window.MinHeightProperty, minHeight);
            }

            var maxHeight = mainView.MaxHeight;
            if (!double.IsNaN(maxHeight))
            {
                window.SetCurrentValue(Window.MaxHeightProperty, maxHeight);
            }

            var minWidth = mainView.MinWidth;
            if (!double.IsNaN(minWidth))
            {
                window.SetCurrentValue(Window.MinWidthProperty, minWidth);
            }

            var maxWidth = mainView.MaxWidth;
            if (!double.IsNaN(maxWidth))
            {
                window.SetCurrentValue(Window.MaxWidthProperty, maxWidth);
            }
        }
    }
}
