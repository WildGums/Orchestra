// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CanvasViewbox.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Markup
{
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;
    using Catel;
    using Catel.Logging;
    using Path = System.Windows.Shapes.Path;

    /// <summary>
    /// Markup extension that can show a canvas inside a viewbox.
    /// </summary>
    public class CanvasViewbox : Catel.Windows.Markup.UpdatableMarkupExtension
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private string _pathName;

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
        public string PathName
        {
            get { return _pathName; }
            set
            {
                _pathName = value;
                UpdateValue();
            }
        }

        protected override object ProvideDynamicValue()
        {
            return GetImageSource();
        }

        private Viewbox GetImageSource()
        {
            var viewbox = new Viewbox();

            viewbox.Stretch = Stretch.Uniform;

            Canvas canvas = null;

            var pathName = PathName;
            if (!string.IsNullOrWhiteSpace(pathName))
            {
                canvas = System.Windows.Application.Current.FindResource(pathName) as Canvas;
                if (canvas != null)
                {
                    // Clone to prevent the same instance to be used multiple times
                    canvas = canvas.Clone();

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
                }
                else
                {
                    Log.Warning("Could not find a resource named '{0}'", pathName);
                }
            }

            viewbox.Child = canvas;

            return viewbox;
        }
    }
}