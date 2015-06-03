// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFlyoutService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
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