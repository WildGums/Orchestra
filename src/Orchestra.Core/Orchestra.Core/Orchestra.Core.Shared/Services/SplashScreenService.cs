// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplashScreenService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;

    public class SplashScreenService : ISplashScreenService
    {
        /// <summary>
        /// Creates the splash screen.
        /// </summary>
        /// <returns>The window.</returns>
        public Window CreateSplashScreen()
        {
            var splashScreen = new Views.SplashScreen();

            return splashScreen;
        }
    }
}