// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutServiceExtensions.cs" company="Simulation Modelling Services">
//   Copyright (c) 2008 - 2014 Simulation Modelling Services. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FallDownMatrixManager.Services
{
    using Catel;
    using Catel.MVVM.Providers;
    using MahApps.Metro.Controls;

    public static class FlyoutServiceExtensions
    {
        public static void AddFlyout<TView>(this IFlyoutService flyoutService, string name, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel)
        {
            Argument.IsNotNull(() => flyoutService);

            flyoutService.AddFlyout(name, typeof(TView), position, unloadBehavior);
        }
    }
}