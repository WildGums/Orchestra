namespace Orchestra.Views
{
    using Catel;    
    using Models;
    using Models.Interface;
    using Xceed.Wpf.AvalonDock;
    using Xceed.Wpf.AvalonDock.Layout;    

    /// <summary>
    /// Interaction logic for NestedDockingManager.xaml
    /// </summary>
    public partial class NestedDockingManager : IDockingManagerContainer
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedDockingManager"/> class.
        /// </summary>
        public NestedDockingManager()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Gets the <see cref="DockingManager" />.
        /// </summary>
        /// <value>
        /// The <see cref="DockingManager" />.
        /// </value>
        public DockingManager DockingManager 
        {
            get { return this.dockingManager; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive
        {
            get
            {
                return IsVisible;
            }
        }

        /// <summary>
        /// Adds the document as a tab to the NestedDocking view.
        /// </summary>
        /// <param name="documentView">The document view.</param>
        /// <param name="dockLocation">The dock location.</param>
        public void AddDocument(LayoutAnchorable documentView, DockLocation? dockLocation = null)
        {
            Argument.IsNotNull( () => documentView);

            if (dockLocation == null)
            {
                layoutDocumentPane.Children.Add(documentView);
                return;
            }

            if (dockLocation == DockLocation.Right)
            {
                rightPropertiesPane.Children.Add(documentView);
            }
            else if (dockLocation == DockLocation.Left)
            {
                leftPropertiesPane.Children.Add(documentView);
            }
            else if (dockLocation == DockLocation.Bottom)
            {
                bottomPropertiesPane.Children.Add(documentView);
            }
            else
            {
                topPropertiesPane.Children.Add(documentView);
            }
        }        
    }
}
