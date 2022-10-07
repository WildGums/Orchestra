namespace Orchestra.Services
{
    using System.Windows;

    public interface IRibbonService : IShellContentService
    {
        FrameworkElement? GetRibbon();
    }
}
