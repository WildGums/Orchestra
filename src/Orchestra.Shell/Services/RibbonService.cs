// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RibbonService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;
    using AvalonDock.Layout;
    using Catel;
    using Catel.Logging;
    using Fluent;
    using Models;
    using Views;

    /// <summary>
    /// The ribbon service.
    /// </summary>
    public class RibbonService : ServiceBase, IRibbonService
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly LayoutDocumentPane _layoutDocumentPane;
        private readonly Dictionary<Type, List<IRibbonItem>> _viewSpecificRibbonItems = new Dictionary<Type, List<IRibbonItem>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonService" /> class.
        /// </summary>
        /// <param name="layoutDocumentPane">The layout document pane.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="layoutDocumentPane"/> is <c>null</c>.</exception>
        public RibbonService(LayoutDocumentPane layoutDocumentPane)
        {
            Argument.IsNotNull("layoutDocumentPane", layoutDocumentPane);

            _layoutDocumentPane = layoutDocumentPane;
            _layoutDocumentPane.PropertyChanged += OnLayoutDocumentPanePropertyChange;
        }

        /// <summary>
        /// Registers the specified ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The <c>Command</c> property of the <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        public void RegisterRibbonItem(IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbonItem", ribbonItem);
            Argument.IsSupported(ribbonItem.Command != null, "When registering a non-view-specific ribbon item, the Command property cannot be null");

            AddRibbonItem(ribbonItem);
        }

        /// <summary>
        /// Registers the ribbon item bound to a specific view type.
        /// </summary>
        /// <typeparam name="TView">The type of the T view.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        public void RegisterViewSpecificRibbonItem<TView>(IRibbonItem ribbonItem)
            where TView : DocumentView
        {
            RegisterViewSpecificRibbonItem(typeof(TView), ribbonItem);
        }

        /// <summary>
        /// Registers the ribbon item bound to a specific view type.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        public void RegisterViewSpecificRibbonItem(Type viewType, IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("viewType", viewType);
            Argument.IsNotNull("ribbonItem", ribbonItem);

            if (!_viewSpecificRibbonItems.ContainsKey(viewType))
            {
                _viewSpecificRibbonItems[viewType] = new List<IRibbonItem>();
            }

            _viewSpecificRibbonItems[viewType].Add(ribbonItem);

            if (!ribbonItem.OnlyShowWhenTabIsActivated)
            {
                AddRibbonItem(ribbonItem);
            }
        }

        /// <summary>
        /// Adds a new ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        private void AddRibbonItem(IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbonItem", ribbonItem);

            Log.Debug("Adding ribbon item '{0}'", ribbonItem);

            var ribbon = GetService<Ribbon>();

            var tab = ribbon.EnsureTabItem(ribbonItem.TabItemHeader);
            var group = tab.EnsureGroupBox(ribbonItem.GroupBoxHeader);

            if (ribbonItem.Command != null)
            {
                group.AddButton(ribbonItem.ItemHeader, ribbonItem.ItemImage, ribbonItem.ItemImage, ribbonItem.Command);
            }
            else
            {
                group.AddButton(ribbonItem.ItemHeader, ribbonItem.ItemImage, ribbonItem.ItemImage, ribbonItem.CommandName);
            }

            Log.Debug("Added ribbon item '{0}'", ribbonItem);
        }

        /// <summary>
        /// Removes the specified ribbon item to the main ribbon.
        /// <para />
        /// This method will ignore calls when the item is not available in the ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        private void RemoveRibbonItem(IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbonItem", ribbonItem);

            Log.Debug("Removing ribbon '{0}'", ribbonItem);

            var ribbon = GetService<Ribbon>();
            ribbon.RemoveItem(ribbonItem);

            Log.Debug("Removed ribbon '{0}'", ribbonItem);
        }

        private void OnLayoutDocumentPanePropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "SelectedContent"))
            {
                Log.Debug("SelectedContent changed, activating the right ribbon");

                // TODO: How to unsubscribe previous actions?

                var ribbon = GetService<Ribbon>();

                var selectedContent = _layoutDocumentPane.SelectedContent;
                if (selectedContent == null)
                {
                    Log.Debug("SelectedContent is null, cannot handle this change");
                    return;
                }

                var documentView = selectedContent.Content as IDocumentView;
                if (documentView == null)
                {
                    Log.Debug("SelectedContent is not a document view, selecting home ribbon tab");

                    ribbon.SelectTabItem("Home");
                    return;
                }

                var documentViewType = documentView.GetType();

                // TODO: Should we add view specific ribbon items?

                if (_viewSpecificRibbonItems.ContainsKey(documentViewType))
                {
                    var firstRibbonItem = _viewSpecificRibbonItems[documentViewType][0];

                    Log.Debug("Selecting ribbon tab '{0}'", firstRibbonItem.TabItemHeader);

                    var selectedTab = ribbon.SelectTabItem(firstRibbonItem.TabItemHeader);
                    if (selectedTab != null)
                    {
                        Log.Debug("Selected tab is a valid ribbon tab, updating data context");

                        var tabItemBinding = new Binding("ViewModel");
                        tabItemBinding.Source = documentView;
                        selectedTab.SetBinding(RibbonTabItem.DataContextProperty, tabItemBinding);
                    }
                }
            }
        }
    }
}