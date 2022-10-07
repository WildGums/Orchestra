namespace Orchestra.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Documents;
    using Catel.Windows.Controls;
    using Tooltips;

    public class AdorneredTooltipsCollection : IAdorneredTooltipsCollection
    {
        private readonly Dictionary<FrameworkElement, IList<IAdorneredTooltip>> _adorneredTooltips;
        private readonly IAdorneredTooltipFactory _factory;

        public AdorneredTooltipsCollection(IAdorneredTooltipFactory factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            _factory = factory;
            _adorneredTooltips = new Dictionary<FrameworkElement, IList<IAdorneredTooltip>>();
        }

        public void Add(FrameworkElement parentControl, Adorner tooltip, bool adornerLayerVisibility)
        {
            if (!Contains(parentControl))
            {
                CreateAdornersList(parentControl);
            }

            GetAdorners(parentControl).Add(_factory.Create(tooltip, adornerLayerVisibility));
        }

        public void Remove(FrameworkElement parentControl)
        {
            if (!Contains(parentControl))
            {
                return;
            }

            _adorneredTooltips.Remove(parentControl);

            var userControl = parentControl as UserControl;
            if (userControl is not null)
            {
                userControl.Unloaded -= OnUnloaded;
            }
        }

        public IList<IAdorneredTooltip> GetAdorners(FrameworkElement parentControl)
        {
            return Contains(parentControl) ? _adorneredTooltips[parentControl] : new List<IAdorneredTooltip>();
        }

        public void ShowAll()
        {
            foreach (var tooltip in _adorneredTooltips.Values.SelectMany(tooltips => tooltips))
            {
                tooltip.Visible = true;
            }
        }

        public void HideAll()
        {
            foreach (var tooltip in _adorneredTooltips.Values.SelectMany(tooltips => tooltips))
            {
                tooltip.Visible = false;
            }
        }

        public void AdornerLayerEnabled()
        {
            foreach (var tooltip in _adorneredTooltips.Values.SelectMany(tooltips => tooltips))
            {
                tooltip.AdornerLayerVisible = true;
            }
        }

        public void AdornerLayerDisabled()
        {
            foreach (var tooltip in _adorneredTooltips.Values.SelectMany(tooltips => tooltips))
            {
                tooltip.AdornerLayerVisible = false;
            }
        }

        private void CreateAdornersList(FrameworkElement parentControl)
        {
            _adorneredTooltips.Add(parentControl, new List<IAdorneredTooltip>());

            parentControl.Unloaded += OnUnloaded;
        }

        private bool Contains(FrameworkElement parentControl)
        {
            return _adorneredTooltips.ContainsKey(parentControl);
        }

        private void OnUnloaded(object? sender, RoutedEventArgs e)
        {
            var parentControl = sender as FrameworkElement;
            if (parentControl is null)
            {
                return;
            }

            Remove(parentControl);
        }
    }
}
