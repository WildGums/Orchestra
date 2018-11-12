namespace Orchestra.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This code originally comes from the WindowsCommunityToolkit (MIT license)
    /// </remarks>
    public class AlignmentGrid : ContentControl
    {
        private readonly Canvas _containerCanvas = new Canvas();

        /// <summary>
        /// Initializes a new instance of the <see cref="AlignmentGrid"/> class.
        /// </summary>
        public AlignmentGrid()
        {
            Loaded += OnLoaded;
            SizeChanged += OnAlignmentGridSizeChanged;

            IsHitTestVisible = false;

            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;
            Content = _containerCanvas;
        }

        /// <summary>
        /// Gets or sets the step to use horizontally.
        /// </summary>
        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }

        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register(nameof(LineBrush), 
            typeof(Brush), typeof(AlignmentGrid), new PropertyMetadata(null, (sender, e) => ((AlignmentGrid)sender).Rebuild()));


        /// <summary>
        /// Gets or sets the step to use horizontally.
        /// </summary>
        public double HorizontalStep
        {
            get { return (double)GetValue(HorizontalStepProperty); }
            set { SetValue(HorizontalStepProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="HorizontalStep"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalStepProperty = DependencyProperty.Register(nameof(HorizontalStep), 
            typeof(double), typeof(AlignmentGrid), new PropertyMetadata(20.0, (sender, e) => ((AlignmentGrid)sender).Rebuild()));


        /// <summary>
        /// Gets or sets the step to use horizontally.
        /// </summary>
        public double VerticalStep
        {
            get { return (double)GetValue(VerticalStepProperty); }
            set { SetValue(VerticalStepProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="VerticalStep"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalStepProperty = DependencyProperty.Register(nameof(VerticalStep), 
            typeof(double), typeof(AlignmentGrid), new PropertyMetadata(20.0, (sender, e) => ((AlignmentGrid)sender).Rebuild()));


        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Rebuild();
        }

        private void Rebuild()
        {
            _containerCanvas.Children.Clear();

            var horizontalStep = HorizontalStep;
            var verticalStep = VerticalStep;
            var brush = LineBrush ?? ThemeHelper.GetAccentColorBrush(AccentColorStyle.AccentColor4);

            for (double x = 0; x < ActualWidth; x += horizontalStep)
            {
                var line = new Rectangle
                {
                    Width = 1,
                    Height = ActualHeight,
                    Fill = brush
                };

                Canvas.SetLeft(line, x);

                _containerCanvas.Children.Add(line);
            }

            for (double y = 0; y < ActualHeight; y += verticalStep)
            {
                var line = new Rectangle
                {
                    Width = ActualWidth,
                    Height = 1,
                    Fill = brush
                };

                Canvas.SetTop(line, y);

                _containerCanvas.Children.Add(line);
            }
        }

        private void OnAlignmentGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rebuild();
        }
    }
}
