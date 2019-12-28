// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonBackstageButton.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System.Windows;
    using Fluent;
    using Button = System.Windows.Controls.Button;

    public class RibbonBackstageButton : Button
    {
        public bool ShowBorder
        {
            get { return (bool)GetValue(ShowBorderProperty); }
            set { SetValue(ShowBorderProperty, value); }
        }

        public static readonly DependencyProperty ShowBorderProperty = DependencyProperty.Register(nameof(ShowBorder), typeof(bool),
            typeof(RibbonBackstageButton), new PropertyMetadata(true));


        public bool IsDefinitive
        {
            get { return (bool)GetValue(IsDefinitiveProperty); }
            set { SetValue(IsDefinitiveProperty, value); }
        }

        public static readonly DependencyProperty IsDefinitiveProperty = DependencyProperty.Register(nameof(IsDefinitive), typeof(bool), 
            typeof(RibbonBackstageButton), new UIPropertyMetadata(true));


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
