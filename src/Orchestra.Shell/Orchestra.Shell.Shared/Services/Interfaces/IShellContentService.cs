// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShellContentService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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