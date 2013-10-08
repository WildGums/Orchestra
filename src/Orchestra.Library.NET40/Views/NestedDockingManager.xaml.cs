using System.Windows.Controls;

namespace Orchestra.Views
{
    using Catel;
    using Models;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Interaction logic for NestedDockingManager.xaml
    /// </summary>
    public partial class NestedDockingManager : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NestedDockingManager"/> class.
        /// </summary>
        public NestedDockingManager()
        {
            InitializeComponent();
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
