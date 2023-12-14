namespace Orchestra.Services
{
    using System.Windows;

    public interface IAdorneredTooltipsManager
    {
        bool IsEnabled { get; }

        void AddHintsFor(FrameworkElement element);

        void HideHints();
        void ShowHints();

        void Enable();
        void Disable();
    }
}
