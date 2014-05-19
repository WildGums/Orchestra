// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MahAppsService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.Services
{
    using System.Windows;
    using global::MahApps.Metro.Controls;
    using Orchestra.Services;
    using Views;

    public class MahAppsService : IMahAppsService
    {
        public WindowCommands GetRightWindowCommands()
        {
            return null;
        }

        public FlyoutsControl GetFlyouts()
        {
            return null;
        }

        public FrameworkElement GetMainView()
        {
            return new MainView();
        }
    }
}