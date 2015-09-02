// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShellContentService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;

    public interface IShellContentService
    {
        FrameworkElement GetMainView();

        FrameworkElement GetStatusBar();
    }
}