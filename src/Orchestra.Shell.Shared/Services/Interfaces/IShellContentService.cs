namespace Orchestra
{
    using System.Windows;

    public interface IShellContentService
    {
        FrameworkElement? GetMainView();

        FrameworkElement? GetStatusBar();
    }
}
