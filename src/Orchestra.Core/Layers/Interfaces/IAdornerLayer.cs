namespace Orchestra.Layers
{
    using System.Windows;
    using System.Windows.Documents;

    public interface IAdornerLayer
    {
        void Add(Adorner adorner);
        Adorner[] GetAdorners(UIElement adornedElement);
    }
}
