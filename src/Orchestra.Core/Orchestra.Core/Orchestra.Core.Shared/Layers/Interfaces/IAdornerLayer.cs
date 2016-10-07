// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdornerLayer.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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