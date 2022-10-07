namespace Orchestra.Layers
{
    using System;
    using System.Windows;
    using System.Windows.Documents;

    public class HintsAdornerLayer : IAdornerLayer
    {
        private readonly AdornerLayer _adornerLayer;

        public HintsAdornerLayer(AdornerLayer adornerLayer)
        {
            ArgumentNullException.ThrowIfNull(adornerLayer);

            _adornerLayer = adornerLayer;
        }

        public void Add(Adorner adorner)
        {
            ArgumentNullException.ThrowIfNull(adorner);

            _adornerLayer.Add(adorner);
        }

        public Adorner[] GetAdorners(UIElement adornedElement)
        {
            return _adornerLayer.GetAdorners(adornedElement);
        }
    }
}
