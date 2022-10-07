namespace Orchestra.Services
{
    using System.Windows;

    public interface IShellContentService
    {
        FrameworkElement? GetMainView();

        FrameworkElement? GetStatusBar();
    }
}
