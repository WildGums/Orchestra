// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdorneredTooltipsManager.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;
    using System.Windows.Documents;
    using Catel;
    using Catel.Windows;
    using Collections;
    using Layers;
    using Models;

    public class AdorneredTooltipsManager : IAdorneredTooltipsManager
    {
        #region Fields
        private readonly IAdornerLayer _adornerLayer;
        private readonly IAdornerTooltipGenerator _adornerTooltipGenerator;
        private readonly IAdorneredTooltipsCollection _adorneredTooltipsCollection;
        private readonly IHintsProvider _hintsProvider;
        #endregion

        #region Constructors
        public AdorneredTooltipsManager(IAdornerTooltipGenerator adornerTooltipGenerator, IHintsProvider hintsProviderProvider,
            IAdornerLayer adornerLayer, IAdorneredTooltipsCollection adorneredTooltipsCollection)
        {
            Argument.IsNotNull(() => adornerTooltipGenerator);
            Argument.IsNotNull(() => hintsProviderProvider);
            Argument.IsNotNull(() => adornerLayer);
            Argument.IsNotNull(() => adorneredTooltipsCollection);

            _adornerTooltipGenerator = adornerTooltipGenerator;
            _hintsProvider = hintsProviderProvider;
            _adornerLayer = adornerLayer;
            _adorneredTooltipsCollection = adorneredTooltipsCollection;

            IsEnabled = true;
        }
        #endregion

        #region IAdorneredTooltipsManager Members
        public void AddHintsFor(FrameworkElement element)
        {
            var triggerHints = _hintsProvider.GetHintsFor(element);
            if (triggerHints == null)
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
        #endregion

        #region Methods
        private UIElement FindElement(FrameworkElement element, IHint hint)
        {
            Argument.IsNotNull(() => element);
            Argument.IsNotNull(() => hint);

            var dependencyObject = (DependencyObject) element;

            return dependencyObject.FindVisualDescendant(o => (o is FrameworkElement) && string.Equals(((FrameworkElement)o).Name, hint.ControlName)) as UIElement;
        }

        private void AddAdorneredTooltip(FrameworkElement element, IHint hint)
        {
            var elementWithHint = FindElement(element, hint);
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
            if (adornedElement == null)
            {
                return false;
            }

            var adorners = _adornerLayer.GetAdorners(adornedElement);
            if (adorners == null)
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
            var adorner = _adornerTooltipGenerator.GetAdornerTooltip(hint, adornedElement);
            adorner.Visibility = !IsEnabled ? Visibility.Collapsed : Visibility.Visible;

            return adorner;
        }
        #endregion
    }
}