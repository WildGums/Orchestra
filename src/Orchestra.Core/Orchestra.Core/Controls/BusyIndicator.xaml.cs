// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusyIndicator.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System.Windows;
    using System.Windows.Media;
    using Catel.Windows;
    using Catel.Windows.Controls;
    using WPFSpark;

    /// <summary>
    /// Interaction logic for BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator : VisualWrapper
    {
        #region Fields
        private MediaElementThreadInfo _mediaElementThreadInfo;

        private Brush _foreground;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BusyIndicator"/> class.
        /// </summary>
        public BusyIndicator()
        {
            InitializeComponent();

            _foreground = Foreground;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
        #endregion

        #region Properties
        public Brush Foreground
        {
            get { return (Brush) GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush),
            typeof(BusyIndicator), new PropertyMetadata(Brushes.White, (sender, e) => ((BusyIndicator)sender)._foreground = e.NewValue as Brush));
        #endregion

        #region Methods
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_mediaElementThreadInfo != null)
            {
                return;
            }

            _mediaElementThreadInfo = MediaElementThreadFactory.CreateMediaElementsOnWorkerThread(CreateBusyIndicator);
            Child = _mediaElementThreadInfo.HostVisual;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_mediaElementThreadInfo != null)
            {
                _mediaElementThreadInfo.Dispose();
                _mediaElementThreadInfo = null;
            }
        }

        private FrameworkElement CreateBusyIndicator()
        {
            var fluidProgressBar = new FluidProgressBar
            {
                Width = ActualWidth,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Foreground = _foreground
            };

            return fluidProgressBar;
        }
        #endregion
    }
}