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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using AvalonDock.Layout;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM.Services;
    using Fluent;
    using Models;
    using Modules;
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
        private readonly IDispatcherService _dispatcherService;
        private readonly Ribbon _ribbon;
        private readonly Dictionary<Type, List<IRibbonItem>> _viewSpecificRibbonItems = new Dictionary<Type, List<IRibbonItem>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RibbonService" /> class.
        /// </summary>
        /// <param name="layoutDocumentPane">The layout document pane.</param>
        /// <param name="dispatcherService">The dispatcher service.</param>
        /// <param name="ribbon">The ribbon.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="layoutDocumentPane"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="dispatcherService"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbon"/> is <c>null</c>.</exception>
        public RibbonService(LayoutDocumentPane layoutDocumentPane, IDispatcherService dispatcherService, Ribbon ribbon)
        {
            Argument.IsNotNull("layoutDocumentPane", layoutDocumentPane);
            Argument.IsNotNull("dispatcherService", dispatcherService);
            Argument.IsNotNull("ribbon", ribbon);

            _layoutDocumentPane = layoutDocumentPane;
            _dispatcherService = dispatcherService;
            _ribbon = ribbon;

            _layoutDocumentPane.PropertyChanged += OnLayoutDocumentPanePropertyChange;
            _ribbon.Loaded += OnRibbonLoaded;
        }

        /// <summary>
        /// Called when the ribbon is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
        /// <remarks>
        /// This is a workaround. If this code is not used, you will get issues with showing the second contextual tab group.
        /// </remarks>
        private void OnRibbonLoaded(object sender, RoutedEventArgs e)
        {
            _ribbon.Loaded -= OnRibbonLoaded;

            foreach (var group in _ribbon.ContextualGroups)
            {
                group.Visibility = Visibility.Visible;
            }

            _dispatcherService.BeginInvoke(() =>
            {
                foreach (var group in _ribbon.ContextualGroups)
                {
                    group.Visibility = Visibility.Hidden;
                }

                ActivateTabForCurrentlySelectedDocumentView();
            });
        }

        /// <summary>
        /// Registers the specified ribbon item to the main ribbon.
        /// </summary>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem" /> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The <c>Command</c> property of the <paramref name="ribbonItem" /> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">The <c>Command</c> property of the <paramref name="ribbonItem" /> is <c>null</c>.</exception>
        public void RegisterRibbonItem(IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbonItem", ribbonItem);
            Argument.IsOfType(() => ribbonItem, typeof(IRibbonButton)); // TODO: consider using IRibbonButton parameter instead of IRibbonItem
            Argument.IsSupported(((IRibbonButton)ribbonItem).Command != null, "When registering a non-view-specific ribbon item, the Command property cannot be null");

            AddRibbonItem(ribbonItem);
        }

        /// <summary>
        /// Registers the ribbon item bound to a specific view type.
        /// </summary>
        /// <typeparam name="TView">The type of the T view.</typeparam>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="contextualTabGroupName">The contextual tab group name.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="contextualTabGroupName"/> is <c>null</c> or whitespace.</exception>
        public void RegisterContextualRibbonItem<TView>(IRibbonItem ribbonItem, string contextualTabGroupName)
            where TView : DocumentView
        {
            RegisterContextualRibbonItem(typeof(TView), ribbonItem, contextualTabGroupName);
        }

        /// <summary>
        /// Registers the ribbon item bound to a specific view type.
        /// </summary>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <param name="contextualTabGroupName">The contextual tab group name.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="viewType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="contextualTabGroupName"/> is <c>null</c> or whitespace.</exception>
        public void RegisterContextualRibbonItem(Type viewType, IRibbonItem ribbonItem, string contextualTabGroupName)
        {
            Argument.IsNotNull("viewType", viewType);
            Argument.IsNotNull("ribbonItem", ribbonItem);

            ribbonItem.ContextualTabItemGroupName = contextualTabGroupName;

            if (!_viewSpecificRibbonItems.ContainsKey(viewType))
            {
                _viewSpecificRibbonItems[viewType] = new List<IRibbonItem>();
            }

            _viewSpecificRibbonItems[viewType].Add(ribbonItem);

            AddRibbonItem(ribbonItem);
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

            RibbonTabItem tab;
            if (ribbonItem.Context == RibbonContext.View)
            {
                tab = ribbon.EnsureContextualTabItem(ribbonItem.TabItemHeader, ribbonItem.ContextualTabItemGroupName);
            }
            else
            {
                tab = ribbon.EnsureTabItem(ribbonItem.TabItemHeader);
            }

            var group = tab.EnsureGroupBox(ribbonItem.GroupBoxHeader);

            Control ribbonItemControl = null;

            var ribbonButton = ribbonItem as IRibbonButton;
            if (ribbonButton != null)
            {
                if (ribbonButton.Command != null)
                {
                    ribbonItemControl = group.AddButton(ribbonButton.ItemHeader, ribbonButton.ItemImage, ribbonButton.ItemImage, ribbonButton.Command);
                }
                else
                {
                    ribbonItemControl = group.AddButton(ribbonButton.ItemHeader, ribbonButton.ItemImage, ribbonButton.ItemImage, ribbonButton.CommandName);
                }
            }
            var ribbonComboBox = ribbonItem as IRibbonComboBox;

            if (ribbonComboBox != null)
            {
                ribbonItemControl = group.AddComboBox(ribbonComboBox.ItemHeader, ribbonComboBox.ItemsSource, ribbonComboBox.SelectedItem);
            }

            var ribbonContentControl = ribbonItem as IRibbonContentControl;

            if (ribbonContentControl != null)
            {
                ribbonItemControl = group.AddContentControl(ribbonItem.ItemHeader, ribbonContentControl.ContentTemplate);
            }

            if (ribbonItemControl != null)
            {
                if (ribbonItem.Layout != null)
                {
                    group.ApplyLayout(ribbonItemControl, ribbonItem.Layout);
                }

                if (ribbonItem.Style != null)
                {
                    ribbonItemControl.Style = ribbonItem.Style;
                }
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
                Log.Debug("SelectedContent changed, activating the right ribbon tabs");

                Log.Debug("Clearing data context of ribbon");

                _ribbon.ClearValue(Ribbon.DataContextProperty);

                Log.Debug("Hiding all contextual groups");

                foreach (var contextualTabGroup in _ribbon.ContextualGroups)
                {
                    contextualTabGroup.Visibility = Visibility.Collapsed;
                }

                ActivateTabForCurrentlySelectedDocumentView();
            }
        }

        private void ActivateTabForCurrentlySelectedDocumentView()
        {
            var selectedContent = _layoutDocumentPane.SelectedContent;
            if (selectedContent == null)
            {
                Log.Debug("SelectedContent is null, cannot activate tab for no selection");
                return;
            }

            var documentView = selectedContent.Content as IDocumentView;
            if (documentView == null)
            {
                Log.Debug("SelectedContent is not a document view, selecting home ribbon tab");

                _ribbon.SelectTabItem(ModuleBase.HomeRibbonTabName);
                return;
            }

            var documentViewType = documentView.GetType();

            if (!_viewSpecificRibbonItems.ContainsKey(documentViewType))
            {
                return;
            }

            var viewSpecificRibbonItems = _viewSpecificRibbonItems[documentViewType];
            foreach (var viewSpecificRibbonItem in viewSpecificRibbonItems)
            {
                if (viewSpecificRibbonItem.Context == RibbonContext.View)
                {
                    Log.Debug("Adding ribbon item '{0}' which has a view context", viewSpecificRibbonItem);

                    var contextualGroup = _ribbon.EnsureContextualTabGroup(viewSpecificRibbonItem.ContextualTabItemGroupName);
                    contextualGroup.Visibility = Visibility.Visible;
                }
            }

            var lastRibbonItem = viewSpecificRibbonItems.Last();
            if (lastRibbonItem.Behavior == RibbonBehavior.ActivateTab)
            {
                Log.Debug("Updating data context of ribbon");

                var ribbonBinding = new Binding("ViewModel");
                ribbonBinding.Source = documentView;
                _ribbon.SetBinding(Ribbon.DataContextProperty, ribbonBinding);

                _dispatcherService.BeginInvoke(() => _ribbon.SelectTabItem(lastRibbonItem.TabItemHeader));
            }
        }
    }
}