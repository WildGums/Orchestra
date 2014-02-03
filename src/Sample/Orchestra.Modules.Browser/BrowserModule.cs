// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowserModule.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.Browser
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using Catel;
    using Catel.IoC;
    using Catel.Linq;
    using Catel.MVVM;
    using Models;
    using Orchestra.ViewModels;
    using Properties;
    using Services;
    using ViewModels;
    using Views;

    /// <summary>
    /// Browser module.
    /// </summary>
    public class BrowserModule : ModuleBase
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IOrchestraService _orchestraService;

        /// <summary>
        /// The module name.
        /// </summary>
        public static readonly string Name = BrowserModuleResources.ModuleName;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserModule"/> class. 
        /// </summary>
        public BrowserModule(IViewModelFactory viewModelFactory, IOrchestraService orchestraService)
            : base(Name)
        {
            _viewModelFactory = viewModelFactory;
            _orchestraService = orchestraService;
        }

        /// <summary>
        /// Called when the module has been initialized.
        /// </summary>
        protected override void OnInitialized()
        {
            // var orchestraService = GetService<IOrchestraService>();
            // orchestraService.ShowDocument<BrowserViewModel>();
        }

        /// <summary>
        /// Initializes the ribbon.
        /// <para />
        /// Use this method to hook up views to ribbon items.
        /// </summary>
        /// <param name="ribbonService">The ribbon service.</param>
        protected override void InitializeRibbon(IRibbonService ribbonService)
        {
            LoadResourceDictionary();

            // Module specific
            ribbonService.RegisterRibbonItem(new RibbonButton(Library.Properties.Resources.HomeRibbonTabName, ModuleName, BrowserModuleResources.OpenNewBrowserModuleMenuItem, new Command(() =>
            {
                var browserViewModel = _viewModelFactory.CreateViewModel<BrowserViewModel>(null);
                _orchestraService.ShowDocument(browserViewModel);
            })) { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_browse.png" });

            // View specific
            var backButton = new RibbonButton(Name, Name, BrowserModuleResources.GoBackMenuItem, "GoBack") { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_left.png" };
            var forwardButton = new RibbonButton(Name, Name, BrowserModuleResources.GoForwardMenuItem, "GoForward") { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_right.png" };
            ribbonService.RegisterContextualRibbonItem<BrowserView>(backButton, ModuleName);
            ribbonService.RegisterContextualRibbonItem<BrowserView>(forwardButton, ModuleName);
            ribbonService.RegisterContextualRibbonItem<BrowserView>(new RibbonButton(Name, Name, BrowserModuleResources.BrowseMenuItem, "Browse") { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_browse.png" }, ModuleName);
            ribbonService.RegisterContextualRibbonItem<BrowserView>(new RibbonSplitButton(Name, Name, BrowserModuleResources.CloseMenuItem, "CloseBrowser")
            {
                ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_close.png",
                ToolTip = new RibbonToolTip { Title = BrowserModuleResources.CloseRibbonToolTipTitle, Text = BrowserModuleResources.CloseRibbonToolTipText },
                Items = new List<IRibbonItem> 
                { 
                    new RibbonGallery
                    {
                        Items = new List<IRibbonItem> { backButton, forwardButton },
                        Orientation = Orientation.Horizontal,
                        ItemWidth = 56, ItemHeight = 64
                    } 
                }
            }, ModuleName);
            ribbonService.RegisterContextualRibbonItem<BrowserView>(new RibbonComboBox(Name, BrowserModuleResources.RecentSitesMenuItem)
            {
                ItemsSource = "RecentSites",
                SelectedItem = "SelectedSite",
                Layout = new RibbonItemLayout { Width = 150 },
                Style = Application.Current.Resources["SelectedSitesComboBoxStyle"] as Style
            }, ModuleName);

            // Find the template to show as dynamic content. TODO: Refactor, make more elegant.
            var template = Application.Current.Resources["TestTemplate"] as DataTemplate;

            //ribbonService.RegisterContextualRibbonItem<BrowserView>(new RibbonContentControl(Name, "Dynamic content") { ContentTemplate = template, Layout = new RibbonItemLayout {Width = 120}}, ModuleName);            

            ribbonService.RegisterRibbonItem(new RibbonButton(Library.Properties.Resources.ViewRibbonTabName, ModuleName, BrowserModuleResources.BrowserPropertiesViewHeader, new Command(() =>
            {
                _orchestraService.ShowDocumentIfHidden<PropertiesViewModel>();
            })) { ItemImage = "/Orchestra.Modules.Browser;component/Resources/Images/action_browse.png" });

            var dockingSettings = new DockingSettings();
            dockingSettings.DockLocation = DockLocation.Right;
            dockingSettings.Width = 225;

            // Demo: register contextual view related to browserview
            var contextualViewModelManager = GetService<IContextualViewModelManager>();
            contextualViewModelManager.RegisterContextualView<BrowserViewModel, PropertiesViewModel>(dockingSettings);

            // Demo: show two pages with different tags
            var orchestraViewModel = _viewModelFactory.CreateViewModel<BrowserViewModel>("Orchestra");
            orchestraViewModel.Url = "http://www.github.com/Orcomp/Orchestra";
            _orchestraService.ShowDocument(orchestraViewModel, "orchestra");

            var catelViewModel = _viewModelFactory.CreateViewModel<BrowserViewModel>("Catel");
            catelViewModel.Url = "http://www.catelproject.com";
            _orchestraService.ShowDocument(catelViewModel, "catel");
        }

        private void LoadResourceDictionary()
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("/Orchestra.Modules.Browser;component/ResourceDictionary.xaml", UriKind.RelativeOrAbsolute) });
        }
    }
}