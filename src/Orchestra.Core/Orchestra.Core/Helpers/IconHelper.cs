// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IconHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Helpers
{
    using System.Drawing;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Catel;

    internal static class IconHelper
    {
        #region Methods
        public static ImageSource GetApplicationIcon(string executablePath, int requiredSize = 64)
        {
            Argument.IsNotNull(() => executablePath);

            var icon = Icon.ExtractAssociatedIcon(executablePath);

            var imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(requiredSize, requiredSize));
            return imageSource;
        }
        #endregion
    }
}