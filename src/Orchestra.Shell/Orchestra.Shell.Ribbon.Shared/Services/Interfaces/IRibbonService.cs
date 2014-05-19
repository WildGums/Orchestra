// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Shell.Services
{
    using System.Windows;

    public interface IRibbonService
    {
        FrameworkElement GetRibbon();

        FrameworkElement GetMainView();
    }
}