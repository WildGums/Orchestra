// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdornerTooltipGenerator.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;
    using System.Windows.Documents;
    using Models;

    public interface IAdornerTooltipGenerator
    {
        #region Methods
        Adorner GetAdornerTooltip(IHint hint, UIElement adornedElement);
        #endregion
    }
}