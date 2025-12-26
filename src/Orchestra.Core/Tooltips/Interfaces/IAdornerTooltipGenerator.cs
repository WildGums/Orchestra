namespace Orchestra
{
    using System.Windows;
    using System.Windows.Documents;

    public interface IAdornerTooltipGenerator
    {
        Adorner GetAdornerTooltip(IHint hint, UIElement adornedElement);
    }
}
