// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Catel;
    using Catel.MVVM.Providers;
    using MahApps.Metro.Controls;

    public static class FlyoutServiceExtensions
    {
        [ObsoleteEx(ReplacementTypeOrMember = "AddFlyout<TView>(this IFlyoutService, string, Position, UnloadBehavior, FlyoutTheme)", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public static void AddFlyout<TView>(this IFlyoutService flyoutService, string name, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel)
        {
            AddFlyout<TView>(flyoutService, name, position, unloadBehavior, FlyoutTheme.Adapt);
        }

        public static void AddFlyout<TView>(this IFlyoutService flyoutService, string name, Position position, 
            UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel, FlyoutTheme flyoutTheme = FlyoutTheme.Adapt)
        {
            Argument.IsNotNull(() => flyoutService);

            flyoutService.AddFlyout(name, typeof(TView), position, unloadBehavior, flyoutTheme);
        }
    }
}