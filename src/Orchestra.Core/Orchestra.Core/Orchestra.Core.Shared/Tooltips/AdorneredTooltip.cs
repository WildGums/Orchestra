// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdorneredTooltip.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tooltips
{
    using System.Windows;
    using System.Windows.Documents;
    using Catel;

    public class AdorneredTooltip : IAdorneredTooltip
    {
        #region Fields
        private readonly Adorner _adorner;
        private bool _adornerLayerVisible;
        private bool _visible;
        #endregion

        #region Constructors
        public AdorneredTooltip(Adorner adorner, bool adornerLayerVisible)
        {
            Argument.IsNotNull(() => adorner);

            _adornerLayerVisible = adornerLayerVisible;
            _adorner = adorner;
            _visible = _adorner.Visibility == Visibility.Visible;
        }
        #endregion

        #region IAdorneredTooltip Members
        public bool Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                UpdateVisibility();
            }
        }

        public bool AdornerLayerVisible
        {
            get { return _adornerLayerVisible; }
            set
            {
                _adornerLayerVisible = value;
                UpdateVisibility();
            }
        }
        #endregion

        #region Methods
        private void UpdateVisibility()
        {
            _adorner.Visibility = _visible && _adornerLayerVisible ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
    }
}