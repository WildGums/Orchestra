// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutServiceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Catel;
    using Catel.MVVM.Providers;
    using MahApps.Metro.Controls;

    public static class FlyoutServiceExtensions
    {
        public static void AddFlyout<TView>(this IFlyoutService flyoutService, string name, Position position, 
            UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel, FlyoutTheme flyoutTheme = FlyoutTheme.Adapt)
        {
            Argument.IsNotNull(() => flyoutService);

            flyoutService.AddFlyout(name, typeof(TView), position, unloadBehavior, flyoutTheme);
        }
    }
}