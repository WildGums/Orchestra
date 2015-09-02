// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRibbonService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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