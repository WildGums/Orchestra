// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;

    public interface IRibbonService : IShellContentService
    {
        FrameworkElement GetRibbon();
    }
}