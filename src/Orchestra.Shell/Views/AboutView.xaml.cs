namespace Orchestra.Views
{
    using System;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;
    using Catel.Windows;
    using Catel.Windows.Controls;
    using ViewModels;

    /// <summary>
    /// Interaction logic for AboutView.xaml.
    /// </summary>
    public partial class AboutView : DataWindow
    {
        /// <summary>
        /// Resources manager for texts.
        /// </summary>
        private static readonly ResourceManager _textResourceManager;
        /// <summary>
        /// Name of current view.
        /// </summary>
        private static readonly string _viewName;

        static AboutView()
        {
            var assembly = Assembly.GetEntryAssembly();
            _viewName = typeof(AboutView).Name;
            _textResourceManager = new ResourceManager(String.Format("{0}.Resources.Texts", assembly.GetName().Name), assembly);
        }

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
            //fallback to default orchestra image if application does not have one in resources (path in XAML)
            if (imgAboutBG.Source == null)
            {
                Uri bgUri = new Uri("pack://application:,,,/Orchestra.Shell;component/Resources/Images/About.png", UriKind.RelativeOrAbsolute);
                imgAboutBG.Source = BitmapFrame.Create(bgUri);
            }
            TranslateBlockTexts();
        }

        /// <summary>
        ///Get resource with given name for current view.
        /// </summary>
        /// <param name="stringName">Name of resource string.</param>
        /// <returns>String from resource</returns>
        private string GetViewResource(string stringName)
        {
            return _textResourceManager.GetString(String.Format("{1}_{0}", stringName, _viewName));
        }

        /// <summary>
        /// Translates texts controls
        /// </summary>
        private void TranslateBlockTexts()
        {
            TranslateSingleBlockText(tbQuickDescription);
            TranslateSingleBlockText(tbLoadedModules);
            TranslateSingleBlockText(tbAboutLongDescription);
            var numberOfLinks = 0;
            if (int.TryParse(GetViewResource("_numberOfLinks"), out numberOfLinks))
            {
                for (int i = 0; i < numberOfLinks; i++)
                {
                    var url = GetViewResource(string.Format("Link_{0}_Url", i));
                    wpLinksPanel.Children.Add(new LinkLabel()
                    {
                        Content = GetViewResource(string.Format("Link_{0}_Content", i)),
                        Url = url == string.Empty ? null : new Uri(url),
                        ClickBehavior = LinkLabelClickBehavior.OpenUrlInBrowser
                    });
                }
            }
        }

        /// <summary>
        /// Translates text from resource for <see cref="TextBlock"/> control.
        /// </summary>
        /// <param name="textBlock">Given text block.</param>
        private void TranslateSingleBlockText(TextBlock textBlock)
        {
            textBlock.Text = GetViewResource(textBlock.Name);
        }
    }
}