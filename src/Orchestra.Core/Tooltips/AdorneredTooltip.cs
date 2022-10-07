namespace Orchestra.Tooltips
{
    using System;
    using System.Windows;
    using System.Windows.Documents;

    public class AdorneredTooltip : IAdorneredTooltip
    {
        private readonly Adorner _adorner;
        private bool _adornerLayerVisible;
        private bool _visible;

        public AdorneredTooltip(Adorner adorner, bool adornerLayerVisible)
        {
            ArgumentNullException.ThrowIfNull(adorner);

            _adornerLayerVisible = adornerLayerVisible;
            _adorner = adorner;
            _visible = _adorner.Visibility == Visibility.Visible;
        }

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

        private void UpdateVisibility()
        {
            _adorner.SetCurrentValue(UIElement.VisibilityProperty, _visible && _adornerLayerVisible ? Visibility.Visible : Visibility.Collapsed);
        }
    }
}
