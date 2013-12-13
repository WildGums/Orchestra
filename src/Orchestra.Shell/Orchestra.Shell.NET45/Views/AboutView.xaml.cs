namespace Orchestra.Views
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for AboutView.xaml.
    /// </summary>
    public partial class AboutView : DataWindow
    {
        private const string AboutImageLocation = "Resources\\Images\\About.png";
        private const string AboutImageFallbackLocation = "/Orchestra.Shell;component/Resources/Images/About.png";

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutView"/> class.
        /// </summary>
        public AboutView()
            : this(null) { }

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
            InitializeAboutWindow();
        }

        /// <summary>
        /// Initializes the splash screen.
        /// </summary>
        private void InitializeAboutWindow()
        {
            var directory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            try
            {
                string firstAttemptFile = Path.Combine(directory, AboutImageLocation);
                if (File.Exists(firstAttemptFile))
                {
                    aboutImage.Source = new BitmapImage(new Uri(firstAttemptFile, UriKind.Absolute));
                    return;
                }
            }
            catch (Exception)
            {
                // Swallow exception
            }

            aboutImage.Source = new BitmapImage(new Uri(AboutImageFallbackLocation, UriKind.RelativeOrAbsolute));
        }
    }
}
