// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasViewbox.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Markup
{
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Markup extension that can show a canvas inside a viewbox.
    /// </summary>
    public class CanvasViewbox : Catel.Windows.Markup.UpdatableMarkupExtension
    {
        public CanvasViewbox()
        {
            Foreground = Brushes.Transparent;
        }

        public CanvasViewbox(string pathName)
            : this()
        {
            PathName = pathName;
        }

        /// <summary>
        /// Gets or sets the foreground. If the foreground is <see cref="Brushes.Transparent"/> (default value), it will
        /// respect the colors of the canvas.
        /// <para />
        /// If this property has a different value, this markup extension will overwrite all the fill colors
        /// of the canvas paths.
        /// </summary>
        public SolidColorBrush Foreground { get; set; }

        /// <summary>
        /// Gets or sets the name of the canvas as it can be found in the application resources.
        /// </summary>
        [ConstructorArgument("pathName")]
        public string PathName { get; set; }

        protected override object ProvideDynamicValue()
        {
            return GetImageSource();
        }

        private Viewbox GetImageSource()
        {
            var viewbox = new Viewbox();

            viewbox.Stretch = Stretch.Uniform;

            // TODO: Question: should we clone or not? 
            var canvas = System.Windows.Application.Current.FindResource(PathName) as Canvas;
            if (canvas != null && Foreground != Brushes.Transparent)
            {
                foreach (var child in canvas.Children)
                {
                    var path = child as Path;
                    if (path != null)
                    {
                        path.Fill = Foreground;
                    }
                }
            }
            viewbox.Child = canvas;

            return viewbox;
        }
    }
}