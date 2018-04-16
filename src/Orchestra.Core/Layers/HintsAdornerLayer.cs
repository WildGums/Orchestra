// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HintsAdornerLayer.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Layers
{
    using System.Windows;
    using System.Windows.Documents;
    using Catel;

    public class HintsAdornerLayer : IAdornerLayer
    {
        #region Fields
        private readonly AdornerLayer _adornerLayer;
        #endregion

        #region Constructors
        public HintsAdornerLayer(AdornerLayer adornerLayer)
        {
            Argument.IsNotNull(() => adornerLayer);

            _adornerLayer = adornerLayer;
        }
        #endregion

        #region IAdornerLayer Members
        public void Add(Adorner adorner)
        {
            _adornerLayer.Add(adorner);
        }

        public Adorner[] GetAdorners(UIElement adornedElement)
        {
            return _adornerLayer.GetAdorners(adornedElement);
        }
        #endregion
    }
}