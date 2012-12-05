namespace Orchestra.Views
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using AvalonDock.Layout;
    using Catel;
    using Catel.Data;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Windows.Controls;
    using Catel.Windows.Controls.MVVMProviders.Logic;
    using Fluent;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : RibbonWindow, IView
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Name of the trace output anchorable.
        /// </summary>
        public const string TraceOutputAnchorable = "traceOutputAnchorable";

        private readonly WindowLogic _windowLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _windowLogic = new WindowLogic(this, typeof(MainWindowViewModel));
            _windowLogic.ViewModelChanged += (s, e) => ViewModelChanged.SafeInvoke(this, e);
            _windowLogic.ViewModelPropertyChanged += (s, e) => ViewModelPropertyChanged.SafeInvoke(this, e);
            _windowLogic.TargetControlPropertyChanged += (s, e) => PropertyChanged.SafeInvoke(this, new AdvancedPropertyChangedEventArgs(s, this, e.PropertyName, e.OldValue, e.NewValue));

            var serviceLocator = ServiceLocator.Default;

            serviceLocator.RegisterInstance(this);
            serviceLocator.RegisterInstance(ribbon);
            serviceLocator.RegisterInstance(dockingManager);
            serviceLocator.RegisterInstance(layoutDocumentPane);

            ribbon.EnsureTabItem("Home");
        }

        /// <summary>
        /// Gets the view model that is contained by the container.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public IViewModel ViewModel
        {
            get { return _windowLogic.ViewModel; }
        }

        /// <summary>
        /// Occurs when the <see cref="ViewModel"/> property has changed.
        /// </summary>
        public event EventHandler<EventArgs> ViewModelChanged;

        /// <summary>
        /// Occurs when a property on the <see cref="ViewModel"/> has changed.
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> ViewModelPropertyChanged;

        /// <summary>
        /// Occurs when a property on the container has changed.
        /// </summary>
        /// <remarks>
        /// This event makes it possible to externally subscribe to property changes of a <see cref="DependencyObject"/>
        /// (mostly the container of a view model) because the .NET Framework does not allows us to.
        /// </remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Determines whether the anchorable with the specified name is currently visible.
        /// </summary>
        /// <param name="name">The name of the anchorable.</param>
        /// <returns><c>true</c> if the anchorable with the specified name is visible; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="InvalidOperationException">The anchorable with the specified name cannot be found.</exception>
        public bool IsAnchorableVisible(string name)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            var anchorable = FindAnchorable(name, true);
            return anchorable.IsVisible;
        }

        /// <summary>
        /// Shows the anchorable with the specified name.
        /// </summary>
        /// <param name="name">The name of the anchorable.</param>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="InvalidOperationException">The anchorable with the specified name cannot be found.</exception>
        public void ShowAnchorable(string name)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            var anchorable = FindAnchorable(name, true);
            anchorable.IsVisible = true;
        }

        /// <summary>
        /// Hides the anchorable with the specified name.
        /// </summary>
        /// <param name="name">The name of the anchorable.</param>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="InvalidOperationException">The anchorable with the specified name cannot be found.</exception>
        public void HideAnchorable(string name)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            var anchorable = FindAnchorable(name, true);
            anchorable.IsVisible = false;
        }

        /// <summary>
        /// Finds the anchorable with the specified name.
        /// </summary>
        /// <param name="name">The name of the anchorable.</param>
        /// <param name="throwExceptionWhenNotFound">if set to <c>true</c>, this method will throw an <see cref="InvalidOperationException"/> when the anchorable cannot be found.</param>
        /// <returns>The <see cref="LayoutAnchorable"/> or <c>null</c> if the anchorable cannot be found.</returns>
        /// <exception cref="ArgumentException">The <paramref name="name"/> is <c>null</c> or whitespace.</exception>
        private LayoutAnchorable FindAnchorable(string name, bool throwExceptionWhenNotFound = false)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            var result = (from anchorable in dockingManager.AnchorablesSource.Cast<LayoutAnchorable>()
                          where TagHelper.AreTagsEqual(anchorable.ContentId, name)
                          select anchorable).FirstOrDefault();

            if (throwExceptionWhenNotFound && result == null)
            {
                string error = string.Format("Anchorable with name '{0}' cannot be found", name);
                Log.Error(error);
                throw new InvalidOperationException(error);
            }

            return result;
        }
    }
}
