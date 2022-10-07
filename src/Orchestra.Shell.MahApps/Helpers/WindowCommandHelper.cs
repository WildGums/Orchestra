// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowCommandHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Catel;

    public static class WindowCommandHelper
    {
        #region Methods
        /// <summary>
        /// Creates the window command button.
        /// </summary>
        /// <param name="packIcon">The content of the button.</param>
        /// <param name="label">The label.</param>
        /// <returns>The right button.</returns>
        public static Button CreateWindowCommandButton(MahApps.Metro.IconPacks.PackIconBase packIcon, string label)
        {
            var button = CreateWindowCommandButton((FrameworkElement)packIcon, label);

            packIcon.SetBinding(MahApps.Metro.IconPacks.PackIconBase.ForegroundProperty, new Binding(nameof(Button.Foreground))
            {
                Source = button,
                Mode = BindingMode.OneWay
            });

            return button;
        }

        /// <summary>
        /// Creates the window command button.
        /// </summary>
        /// <param name="content">The content of the button.</param>
        /// <param name="label">The label.</param>
        /// <returns>The right button.</returns>
        public static Button CreateWindowCommandButton(FrameworkElement content, string label)
        {
            ArgumentNullException.ThrowIfNull(content);

            var button = new Button();
            button.Content = content;

            if (!string.IsNullOrEmpty(label))
            {
                button.ToolTip = label;
            }

            return button;
        }
        #endregion
    }
}
