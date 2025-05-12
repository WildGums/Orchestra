namespace Orchestra.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Catel.Windows;
    using Orc.Controls;
    using Orc.Controls.Services;

    public partial class SplashScreen : DataWindow, IStatusRepresenter
    {
#pragma warning disable IDISP006 // Implement IDisposable
        private MediaElementThreadInfo? _mediaElementThreadInfo;
#pragma warning restore IDISP006 // Implement IDisposable

        private Grid? _grid;
        private Orc.Controls.AnimatingTextBlock? _animatingTextBlock;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplashScreen" /> class.
        /// </summary>
        public SplashScreen()
            : base(DataWindowMode.Custom)
        {
            InitializeComponent();

            var application = Application.Current;

            Background = (application is not null) ? Orc.Theming.ThemeManager.Current.GetAccentColorBrush() : Brushes.DodgerBlue;
        }

        public void UpdateStatus(string status)
        {
            _animatingTextBlock?.UpdateStatus(status);
        }

        protected override void OnLoaded(EventArgs e)
        {
            base.OnLoaded(e);

            if (_mediaElementThreadInfo is not null)
            {
                return;
            }

#pragma warning disable IDISP003 // Dispose previous before re-assigning
            _mediaElementThreadInfo = MediaElementThreadFactory.CreateMediaElementsOnWorkerThread(CreateAnimatingTextBlock);
#pragma warning restore IDISP003 // Dispose previous before re-assigning

            AnimatingTextBlockVisualWrapper.Child = _mediaElementThreadInfo.HostVisual;
        }

        protected override void OnUnloaded(EventArgs e)
        {
            if (_mediaElementThreadInfo is not null)
            {
                _mediaElementThreadInfo.Dispose();
                _mediaElementThreadInfo = null;
            }

            base.OnUnloaded(e);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_grid is not null && sizeInfo.WidthChanged)
            {
                _grid.Dispatcher.BeginInvoke(() =>
                {
                    _grid.SetCurrentValue(WidthProperty, sizeInfo.NewSize.Width);
                });
            }
        }

        private FrameworkElement CreateAnimatingTextBlock()
        {
            //<orccontrols:AnimatingTextBlock x:Name="statusTextBlock"
            //                                           HorizontalContentAlignment="Center"
            //                                           VerticalContentAlignment="Center"
            //                                           TextAlignment="Center"
            //                                           Margin="70 0 70 0"
            //                                           TextBlockStyle="{StaticResource SplashStatusTextBlockStyle}" />

            var animatingTextBlock = new Orc.Controls.AnimatingTextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(70, 0, 70, 0),
                TextBlockStyle = (Style)Resources["SplashStatusTextBlockStyle"]
            };

            var grid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            grid.Children.Add(animatingTextBlock);

//#if DEBUG
//            animatingTextBlock.Background = Brushes.Black;
//            grid.Background = Brushes.Green;
//#endif

            _animatingTextBlock = animatingTextBlock;
            _grid = grid;

            return grid;
        }
    }
}
