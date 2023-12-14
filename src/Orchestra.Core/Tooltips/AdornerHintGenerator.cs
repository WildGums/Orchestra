namespace Orchestra.Tooltips
{
    using System;
    using System.Windows;
    using System.Windows.Documents;
    using Services;

    internal class AdornerHintGenerator : IAdornerTooltipGenerator
    {
        public Adorner GetAdornerTooltip(IHint hint, UIElement adornedElement)
        {
            ArgumentNullException.ThrowIfNull(hint);
            ArgumentNullException.ThrowIfNull(adornedElement);

            return new TextBlockAdorner(adornedElement, hint);
        }
    }
}
