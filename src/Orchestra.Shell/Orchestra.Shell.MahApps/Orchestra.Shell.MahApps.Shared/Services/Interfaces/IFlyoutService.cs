// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFlyoutService.cs" company="Simulation Modelling Services">
//   Copyright (c) 2008 - 2014 Simulation Modelling Services. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FallDownMatrixManager.Services
{
    using System;
    using System.Collections.Generic;
    using Catel.MVVM.Providers;
    using MahApps.Metro.Controls;

    public interface IFlyoutService
    {
        void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel);
        void ShowFlyout(string name, object dataContext);
        void HideFlyout(string name);
        IEnumerable<Flyout> GetFlyouts();
    }
}