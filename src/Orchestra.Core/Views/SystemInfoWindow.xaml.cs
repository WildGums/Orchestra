namespace Orchestra.Views
{
    using System.Windows;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for SystemInfoWindow.xaml.
    /// </summary>
    public partial class SystemInfoWindow
    {
        partial void OnInitializingComponent()
        {
            Mode = DataWindowMode.Custom;
        }

        private void OnCloseClick(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
