namespace Orchestra.Services
{
    using System.Windows;

    public interface IHintsProvider
    {
        IHint[] GetHintsFor(FrameworkElement element);
    }
}
