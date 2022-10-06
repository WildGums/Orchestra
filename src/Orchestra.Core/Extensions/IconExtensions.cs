// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IconExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Drawing;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Catel;

    public static class IconExtensions
    {
        public static ImageSource ToImageSource(this Icon icon, int requiredSize = 64)
        {
            ArgumentNullException.ThrowIfNull(icon);

            var imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, 
                BitmapSizeOptions.FromWidthAndHeight(requiredSize, requiredSize));

            return imageSource;
        }
    }
}