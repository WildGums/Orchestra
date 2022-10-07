namespace Orchestra.Services
{
    using System;
    using System.Windows;
    using System.Windows.Documents;
    using Catel.Windows;
    using Collections;
    using Layers;

    public class AdorneredTooltipsManager : IAdorneredTooltipsManager
    {
        private readonly IAdornerLayer _adornerLayer;
        private readonly IAdornerTooltipGenerator _adornerTooltipGenerator;
        private readonly IAdorneredTooltipsCollection _adorneredTooltipsCollection;
        private readonly IHintsProvider _hintsProvider;

        public AdorneredTooltipsManager(IAdornerTooltipGenerator adornerTooltipGenerator, IHintsProvider hintsProviderProvider,
            IAdornerLayer adornerLayer, IAdorneredTooltipsCollection adorneredTooltipsCollection)
        {
            ArgumentNullException.ThrowIfNull(adornerTooltipGenerator);
            ArgumentNullException.ThrowIfNull(hintsProviderProvider);
            ArgumentNullException.ThrowIfNull(adornerLayer);
            ArgumentNullException.ThrowIfNull(adorneredTooltipsCollection);

            _adornerTooltipGenerator = adornerTooltipGenerator;
            _hintsProvider = hintsProviderProvider;
            _adornerLayer = adornerLayer;
            _adorneredTooltipsCollection = adorneredTooltipsCollection;

            IsEnabled = true;
        }

        public void AddHintsFor(FrameworkElement element)
        {
            ArgumentNullException.ThrowIfNull(element);

            var triggerHints = _hintsProvider.GetHintsFor(element);
            if (triggerHints is null)
            {
                return;
            }

            foreach (var hint in triggerHints)
            {
                AddAdorneredTooltip(element, hint);
            }
        }

        public void HideHints()
        {
            _adorneredTooltipsCollection.HideAll();
        }

        public void ShowHints()
        {
            _adorneredTooltipsCollection.ShowAll();
        }

        public void Enable()
        {
            IsEnabled = true;

            _adorneredTooltipsCollection.AdornerLayerEnabled();
        }

        public void Disable()
        {
            IsEnabled = false;

            _adorneredTooltipsCollection.AdornerLayerDisabled();
        }

        public bool IsEnabled { get; private set; }

        private UIElement? FindElement(FrameworkElement element, IHint hint)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(hint);

            var dependencyObject = (DependencyObject) element;

            return dependencyObject.FindVisualDescendant(o => (o is FrameworkElement) && string.Equals(((FrameworkElement)o).Name, hint.ControlName)) as UIElement;
        }

        private void AddAdorneredTooltip(FrameworkElement element, IHint hint)
        {
            ArgumentNullException.ThrowIfNull(element);
            ArgumentNullException.ThrowIfNull(hint);

            var elementWithHint = FindElement(element, hint);
            if (elementWithHint is null)
            {
                return;
            }

            if (!CanAddAdorner(elementWithHint))
            {
                return;
            }

            var adornerTooltip = CreateAdornerTooltip(hint, elementWithHint);
            _adornerLayer.Add(adornerTooltip);
            _adorneredTooltipsCollection.Add(element, adornerTooltip, IsEnabled);
        }

        private bool CanAddAdorner(UIElement adornedElement)
        {
            if (adornedElement is null)
            {
                return false;
            }

            var adorners = _adornerLayer.GetAdorners(adornedElement);
            if (adorners is null)
            {
                return true;
            }

            if (adorners.Length != 0)
            {
                return false;
            }

            return true;
        }

        private Adorner CreateAdornerTooltip(IHint hint, UIElement adornedElement)
        {
            ArgumentNullException.ThrowIfNull(hint);
            ArgumentNullException.ThrowIfNull(adornedElement);

            var adorner = _adornerTooltipGenerator.GetAdornerTooltip(hint, adornedElement);

            adorner.SetCurrentValue(UIElement.VisibilityProperty, !IsEnabled ? Visibility.Collapsed : Visibility.Visible);

            return adorner;
        }
    }
}
