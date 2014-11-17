// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowCommandHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
        /// <param name="style">The style.</param>
        /// <param name="label">The label.</param>
        /// <returns>The right button.</returns>
        public static Button CreateWindowCommandButton(string style, string label)
        {
            Argument.IsNotNullOrWhitespace(() => style);

            var button = new Button();
            button.Content = CreateWindowCommandRectangle(button, style);

            if (!string.IsNullOrEmpty(label))
            {
                button.ToolTip = label;
            }

            return button;
        }

        /// <summary>
        /// Creates the window command rectangle.
        /// </summary>
        /// <param name="parentButton">The parent button.</param>
        /// <param name="style">The style.</param>
        /// <returns>Rectangle.</returns>
        public static Rectangle CreateWindowCommandRectangle(Button parentButton, string style)
        {
            Argument.IsNotNull(() => parentButton);
            Argument.IsNotNullOrWhitespace(() => style);

            var rectangle = new Rectangle
            {
                Width = 16d,
                Height = 16d,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Stretch = Stretch.UniformToFill
            };

            rectangle.SetBinding(Rectangle.FillProperty, new Binding("Foreground")
            {
                Source = parentButton
            });

            var application = Application.Current;
            if (application != null)
            {
                rectangle.OpacityMask = new VisualBrush
                {
                    //Stretch = Stretch.Fill,
                    Visual = application.FindResource(style) as Visual
                };
            }

            return rectangle;
        }
        #endregion
    }
}