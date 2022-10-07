namespace Orchestra.Markup
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Shapes;
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
            AllowUpdatableStyleSetters = true;
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

        protected override object? ProvideDynamicValue(IServiceProvider? serviceProvider)
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
                var currentApp = System.Windows.Application.Current;
                if (currentApp is not null)
                {
                    canvas = currentApp.TryFindResource(pathName) as Canvas;
                    if (canvas is not null)
                    {
                        // Clone to prevent the same instance to be used multiple times
                        canvas = canvas.Clone();

                        if (canvas is not null && Foreground != Brushes.Transparent)
                        {
                            foreach (var child in canvas.Children)
                            {
                                var path = child as Path;
                                if (path is not null)
                                {
                                    path.SetCurrentValue(Shape.FillProperty, Foreground);
                                }
                            }
                        }
                    }
                }

                if (canvas is null)
                {
                    Log.Warning("Could not find a resource named '{0}'", pathName);
                }
            }

            viewbox.Child = canvas;

            return viewbox;
        }
    }
}
