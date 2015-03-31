// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdornerHintGenerator.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tooltips
{
    using System.Windows;
    using System.Windows.Documents;
    using Models;
    using Services;

    internal class AdornerHintGenerator : IAdornerTooltipGenerator
    {
        #region Methods
        public Adorner GetAdornerTooltip(IHint hint, UIElement adornedElement)
        {
            return new TextBlockAdorner(adornedElement, hint);
        }
        #endregion
    }
}