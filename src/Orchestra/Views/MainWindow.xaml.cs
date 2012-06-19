namespace Orchestra.Views
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using Catel;
    using Catel.IoC;
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
        private readonly WindowLogic _windowLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _windowLogic = new WindowLogic(this, typeof(MainWindowViewModel));
            _windowLogic.ViewModelChanged += (s, e) => ViewModelChanged.SafeInvoke(s, e);
            _windowLogic.ViewModelPropertyChanged += (s, e) => ViewModelPropertyChanged.SafeInvoke(s, e);
            _windowLogic.PropertyChanged += (s, e) => PropertyChanged.SafeInvoke(s, e);

            var serviceLocator = ServiceLocator.Instance;

            serviceLocator.RegisterInstance(ribbon);
            serviceLocator.RegisterInstance(dockingManager);
            serviceLocator.RegisterInstance(layoutDocumentPane);
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
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;
    }
}
