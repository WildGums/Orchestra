// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISplashScreenService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;

    public interface ISplashScreenService
    {
        /// <summary>
        /// Creates the splash screen.
        /// </summary>
        /// <returns>The window.</returns>
        Window CreateSplashScreen();
    }
}