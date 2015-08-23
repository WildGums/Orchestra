// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusyIndicator.xaml.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Catel.Windows;
    using Catel.Windows.Controls;
    using Catel.Windows.Threading;
    using WPFSpark;

    /// <summary>
    /// Interaction logic for BusyIndicator.xaml
    /// </summary>
    public partial class BusyIndicator : VisualWrapper
    {
        #region Fields
        private MediaElementThreadInfo _mediaElementThreadInfo;
        private Grid _grid;

        private Brush _foreground;
        private int _ignoreUnloadedEventCount;
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

        public int IgnoreUnloadedEventCount
        {
            get { return (int)GetValue(IgnoreUnloadedEventCountProperty); }
            set { SetValue(IgnoreUnloadedEventCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreUnloadedEventCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreUnloadedEventCountProperty = DependencyProperty.Register("IgnoreUnloadedEventCount", 
            typeof(int), typeof(BusyIndicator), new PropertyMetadata(0, (sender, e) => ((BusyIndicator)sender).OnIgnoreUnloadedEventCountChanged()));
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
            if (_ignoreUnloadedEventCount > 0)
            {
                _ignoreUnloadedEventCount--;
                return;
            }

            if (_mediaElementThreadInfo != null)
            {
                _mediaElementThreadInfo.Dispose();
                _mediaElementThreadInfo = null;
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_grid != null)
            {
                if (sizeInfo.WidthChanged)
                {
                    _grid.Dispatcher.BeginInvoke(() =>
                    {
                        _grid.Width = sizeInfo.NewSize.Width;
                    });
                }
            }
        }

        private void OnIgnoreUnloadedEventCountChanged()
        {
            _ignoreUnloadedEventCount = IgnoreUnloadedEventCount;
        }

        private FrameworkElement CreateBusyIndicator()
        {
            var fluidProgressBar = new FluidProgressBar
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                Foreground = _foreground
            };

            var grid = new Grid();
            grid.Width = ActualWidth;
            //grid.Background = Brushes.Red;
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.Children.Add(fluidProgressBar);

            _grid = grid;

            return grid;
        }
        #endregion
    }
}