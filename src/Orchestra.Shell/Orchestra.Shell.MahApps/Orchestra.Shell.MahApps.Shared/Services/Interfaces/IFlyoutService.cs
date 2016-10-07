// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFlyoutService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
        [ObsoleteEx(ReplacementTypeOrMember = "AddFlyout(string, Type, Position, UnloadBehavior, FlyoutTheme)", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel);
        void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel, FlyoutTheme flyoutTheme = FlyoutTheme.Adapt);

        void ShowFlyout(string name, object dataContext);
        void HideFlyout(string name);
        IEnumerable<Flyout> GetFlyouts();
    }
}