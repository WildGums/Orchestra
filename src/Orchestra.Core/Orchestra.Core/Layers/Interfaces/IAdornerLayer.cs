// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdornerLayer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Layers
{
    using System.Windows;
    using System.Windows.Documents;

    public interface IAdornerLayer
    {
        #region Methods
        void Add(Adorner adorner);
        Adorner[] GetAdorners(UIElement adornedElement);
        #endregion
    }
}