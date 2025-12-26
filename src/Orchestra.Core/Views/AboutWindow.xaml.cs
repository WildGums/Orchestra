namespace Orchestra.Views
{
    using System.Windows;
    using Catel.Windows;
    using Windows;

    public partial class AboutWindow
    {
        partial void OnInitializingComponent()
        {
            Mode = DataWindowMode.Custom;

            this.ApplyApplicationIcon();
        }

        private void Close_OnClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
