// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisualExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using Catel.Windows;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Extensions for <see cref="System.Windows.Media.Visual"/>.
    /// </summary>
    public static class VisualExtensions
    {
        #region Methods
        /// <summary>
        /// Get the parent window for this visual object or null when not exists.
        /// </summary>
        /// <param name="visualObject">Reference to visual object.</param>
        /// <returns>Reference to partent window or null when not exists.</returns>
        public static System.Windows.Window GetParentWindow(this Visual visualObject)
        {
            if (visualObject == null)
            {
                return null;
            }

            return visualObject.GetAncestorObject<System.Windows.Window>();
        }
        #endregion
    }
}