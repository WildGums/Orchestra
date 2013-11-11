// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid
{
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Orchestra.Models;
    using Properties;
    using Services;
    using ViewModels;
    using Views;

    /// <summary>
    /// The data grid module.
    /// </summary>
    public class DataGridModule : ModuleBase
    {
        private readonly IOrchestraService _orchestraService;

        #region Constants
        /// <summary>
        /// The module name.
        /// </summary>
        public const string Name = "DataGrid";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridModule" /> class.
        /// </summary>
        public DataGridModule(IOrchestraService orchestraService)
            : base(Name)
        {
            Argument.IsNotNull(() => orchestraService);

            _orchestraService = orchestraService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the license URL.
        /// <para />
        /// If this method returns an empty string, it is assumed the module has no license.
        /// </summary>
        /// <returns>The url of the license.</returns>
        public override string GetLicenseUrl()
        {
            return "https://github.com/Orcomp/Orchestra";
        }

        /// <summary>
        /// The on initialized.
        /// </summary>
        protected override void OnInitialized()
        {            
        }

        /// <summary>
        /// Initializes the ribbon.
        /// <para />
        /// Use this method to hook up views to ribbon items.
        /// </summary>
        /// <param name="ribbonService">The ribbon service.</param>
        protected override void InitializeRibbon(IRibbonService ribbonService)
        {
            var orchestraService = GetService<IOrchestraService>();
            var typeFactory = TypeFactory.Default;

            // Module specific
            ribbonService.RegisterRibbonItem(
                new RibbonButton(OrchestraResources.HomeRibbonTabName, ModuleName, "Open", new Command(() => _orchestraService.ShowDocument<DataGridViewModel>()))
                {
                    ItemImage = "/Orchestra.Modules.DataGrid;component/Resources/Images/Table.png"
                });

            // View specific
            ribbonService.RegisterContextualRibbonItem<DataGridView>(
                new RibbonButton(Name, "File", "Open", "OpenFileCommand") { ItemImage = "/Orchestra.Library;component/Resources/Images/FileOpen.png" },
                ModuleName);
            ribbonService.RegisterContextualRibbonItem<DataGridView>(
                new RibbonButton(Name, "File", "Save", "SaveToFileCommand") { ItemImage = "/Orchestra.Library;component/Resources/Images/FileSave.png" },
                ModuleName);

            ribbonService.RegisterContextualRibbonItem<DataGridView>(
                new RibbonButton(Name, "Rows", "Add", "AddRowCommand") { ItemImage = "/Orchestra.Library;component/Resources/Images/ActionAdd.png" },
                ModuleName);
            ribbonService.RegisterContextualRibbonItem<DataGridView>(
                new RibbonButton(Name, "Rows", "Remove", "RemoveRowCommand") { ItemImage = "/Orchestra.Library;component/Resources/Images/ActionRemove.png" },
                ModuleName);

            ribbonService.RegisterContextualRibbonItem<DataGridView>(
                new RibbonButton(Name, "Tools", "Plot", "Plot") { ItemImage = "/Orchestra.Modules.DataGrid;component/Resources/Images/ActionPlot.png" },
                ModuleName);

            var contextualViewModelManager = GetService<IContextualViewModelManager>();

            // Demo: Register the view as a Nested dockingmanager
            contextualViewModelManager.RegisterNestedDockView<DataGridViewModel>();

            DockingSettings dockingSettings = new DockingSettings();
            dockingSettings.DockLocation = DockLocation.Floating;
            dockingSettings.Top = 100;
            dockingSettings.Left = 450;
            dockingSettings.Width = 200;
            dockingSettings.Height = 200;

            // Demo: Register context sensitive view, within the Nested dockingmanager
            contextualViewModelManager.RegisterContextualView<DataGridViewModel, DataGridPropertiesViewModel>("Datagrid properties", dockingSettings);            

            // Add the contextual view in the "View" menu
            ribbonService.RegisterRibbonItem(new RibbonButton(OrchestraResources.ViewRibbonTabName, ModuleName, "Browser properties", 
                new Command(() => orchestraService.ShowDocumentIfHidden<DataGridPropertiesViewModel>())) { ItemImage = "/Orchestra.Modules.DataGrid;component/Resources/Images/Table.png" });

            // Test showing a datagrid view at startup
           var dataGridViewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<DataGridViewModel>("Datagrid");
           orchestraService.ShowDocument(dataGridViewModel, "Datagrid");            
        }
        #endregion
    }
}