// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonBackstageButton.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System.Windows;
    using Fluent;
    using Button = System.Windows.Controls.Button;
    using MenuItem = System.Windows.Controls.MenuItem;

    public class RibbonBackstageButton : Button
    {
        public bool ShowBorder
        {
            get { return (bool)GetValue(ShowBorderProperty); }
            set { SetValue(ShowBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register("ShowBorder", typeof(bool), 
            typeof(RibbonBackstageButton), new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets whether ribbon control click must close backstage
        /// </summary>
        public bool IsDefinitive
        {
            get { return (bool)GetValue(IsDefinitiveProperty); }
            set { SetValue(IsDefinitiveProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsDefinitive.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty IsDefinitiveProperty =
            DependencyProperty.Register("IsDefinitive", typeof(bool), typeof(MenuItem), new UIPropertyMetadata(true));

        protected override void OnClick()
        {
            // Close popup on click
            if (IsDefinitive)
            {
                PopupService.RaiseDismissPopupEventAsync(this, DismissPopupMode.Always);
            }

            base.OnClick();
        }
    }
}