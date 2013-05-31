namespace Orchestra.Views
{
    using System;
    using System.Windows.Media.Imaging;
    using Catel.Windows;
    using ViewModels;

    /// <summary>
    /// Interaction logic for AboutView.xaml.
    /// </summary>
    public partial class AboutView : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutView"/> class.
        /// </summary>
        public AboutView()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public AboutView(AboutViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();
            LoadBGImage();
        }

        /// <summary>
        /// Loads background image from source in application resources or from Orchestra assembly.
        /// </summary>
        private void LoadBGImage()
        {
            //fallback to default orchestra image if application does not have one in resources (path in XAML) or if is invalid
            if (imgAboutBG.Source == null)
            {
                Uri bgUri = new Uri("pack://application:,,,/Orchestra.Shell;component/Resources/Images/About.png", UriKind.RelativeOrAbsolute);
                imgAboutBG.Source = BitmapFrame.Create(bgUri);
            }
        }
    }
}