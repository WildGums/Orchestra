// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdorneredTooltipsCollection.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Collections
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Documents;
    using Catel;
    using Catel.MVVM.Views;
    using Catel.Windows.Controls;
    using Tooltips;

    public class AdorneredTooltipsCollection : IAdorneredTooltipsCollection
    {
        #region Fields
        private readonly Dictionary<FrameworkElement, IList<IAdorneredTooltip>> _adorneredTooltips;
        private readonly IAdorneredTooltipFactory _factory;
        #endregion

        #region Constructors
        public AdorneredTooltipsCollection(IAdorneredTooltipFactory factory)
        {
            Argument.IsNotNull(() => factory);

            _factory = factory;
            _adorneredTooltips = new Dictionary<FrameworkElement, IList<IAdorneredTooltip>>();
        }
        #endregion

        #region Methods
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
            if (userControl != null)
            {
                userControl.Unloaded -= OnUnloaded;
            }
        }

        public IList<IAdorneredTooltip> GetAdorners(FrameworkElement parentControl)
        {
            return Contains(parentControl) ? _adorneredTooltips[parentControl] : null;
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

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var parentControl = sender as FrameworkElement;
            if (parentControl == null)
            {
                return;
            }

            Remove(parentControl);
        }
        #endregion
    }
}