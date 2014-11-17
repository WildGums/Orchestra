// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonBackstageTabItemHeader.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class RibbonBackstageTabItemHeader : ContentControl
    {
        public RibbonBackstageTabItemHeader()
        {
        }

        public bool KeepIconSizeWithoutIcon
        {
            get { return (bool)GetValue(KeepIconSizeWithoutIconProperty); }
            set { SetValue(KeepIconSizeWithoutIconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeepIconSizeWithoutIcon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeepIconSizeWithoutIconProperty = DependencyProperty.Register("KeepIconSizeWithoutIcon", typeof(bool), typeof(RibbonBackstageTabItemHeader), new PropertyMetadata(false, (sender, e) => ((RibbonBackstageTabItemHeader)sender).BuildHeader()));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource),
            typeof(RibbonBackstageTabItemHeader), new PropertyMetadata(null, (sender, e) => ((RibbonBackstageTabItemHeader)sender).BuildHeader()));

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderText", typeof(string),
            typeof(RibbonBackstageTabItemHeader), new PropertyMetadata(string.Empty, (sender, e) => ((RibbonBackstageTabItemHeader)sender).BuildHeader()));

        public string HeaderTextStyleKey
        {
            get { return (string)GetValue(HeaderTextStyleKeyProperty); }
            set { SetValue(HeaderTextStyleKeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HeaderTextStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderTextStyleKeyProperty = DependencyProperty.Register("HeaderTextStyleKey", typeof(string),
            typeof(RibbonBackstageTabItemHeader), new PropertyMetadata("RibbonBackstageTabItemHeaderLabelStyle",
                (sender, e) => ((RibbonBackstageTabItemHeader)sender).BuildHeader()));

        private void BuildHeader()
        {
            var image = new Image
            {
                Source = Icon,
                Style = TryFindResource("RibbonBackstageTabItemHeaderImageStyle") as Style
            };

            Grid.SetColumn(image, 0);

            var label = new Label
            {
                Content = HeaderText,
                Style = TryFindResource(HeaderTextStyleKey) as Style
            };

            Grid.SetColumn(label, 1);

            var size = (Icon != null) || KeepIconSizeWithoutIcon ? 36 : 0;

            var grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(size, GridUnitType.Pixel)
            });

            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star),
                MinWidth = 200
            });

            grid.Children.Add(image);
            grid.Children.Add(label);

            Content = grid;
        }
    }
}