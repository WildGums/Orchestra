namespace Orchestra
{
    using System.Windows;

    public interface IRibbonService : IShellContentService
    {
        FrameworkElement? GetRibbon();
    }
}
