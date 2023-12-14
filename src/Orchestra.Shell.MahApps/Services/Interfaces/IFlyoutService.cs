namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM.Providers;
    using MahApps.Metro.Controls;

    public interface IFlyoutService
    {
        void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel, FlyoutTheme flyoutTheme = FlyoutTheme.Adapt);

        void ShowFlyout(string name, object dataContext);
        void HideFlyout(string name);
        IEnumerable<Flyout> GetFlyouts();
    }
}
