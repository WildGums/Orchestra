namespace Orchestra.Services
{
    using System;
    using Catel.MVVM.Providers;
    using MahApps.Metro.Controls;

    public static class FlyoutServiceExtensions
    {
        public static void AddFlyout<TView>(this IFlyoutService flyoutService, string name, Position position, 
            UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel, FlyoutTheme flyoutTheme = FlyoutTheme.Adapt)
        {
            ArgumentNullException.ThrowIfNull(flyoutService);

            flyoutService.AddFlyout(name, typeof(TView), position, unloadBehavior, flyoutTheme);
        }
    }
}
