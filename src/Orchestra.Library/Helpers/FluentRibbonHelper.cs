// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentRibbonHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using Catel;
    using Catel.Logging;
    using Fluent;
    using Models;
    using Button = Fluent.Button;
    using ComboBox = Fluent.ComboBox;
    using IRibbonControl = Models.IRibbonControl;

    /// <summary>
    /// Helper class for the <see cref="Ribbon" /> control.
    /// </summary>
    public static class FluentRibbonHelper
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Selects the tab item in the ribbon. If the tab cannot be found, the first tab will be
        /// selected.
        /// </summary>
        /// <param name="ribbon">The ribbon.</param>
        /// <param name="header">The header.</param>
        /// <returns>The selected ribbon tab item.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="ribbon"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        public static RibbonTabItem SelectTabItem(this Ribbon ribbon, string header)
        {
            Argument.IsNotNull("ribbon", ribbon);
            Argument.IsNotNullOrWhitespace("header", header);

            var tabItem = (from tab in ribbon.Tabs
                           where string.Equals(tab.Header.ToString(), header)
                           select tab).FirstOrDefault();

            if (tabItem != null)
            {
                ribbon.SelectedTabItem = tabItem;
            }
            else if (ribbon.Tabs.Count > 0)
            {
                ribbon.SelectedTabIndex = 0;
            }

            return ribbon.SelectedTabItem;
        }

        /// <summary>
        /// Ensures that a contextual tab item group with the specified header exists.
        /// </summary>
        /// <param name="ribbon">The ribbon.</param>
        /// <param name="header">The header.</param>
        /// <returns>The existing or newly created <see cref="RibbonTabItem"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="ribbon"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        public static RibbonContextualTabGroup EnsureContextualTabGroup(this Ribbon ribbon, string header)
        {
            Argument.IsNotNull("ribbon", ribbon);
            Argument.IsNotNullOrWhitespace("header", header);

            var tabGroup = (from tab in ribbon.ContextualGroups
                            where string.Equals(tab.Header, header)
                            select tab).FirstOrDefault();
            if (tabGroup == null)
            {
                tabGroup = new RibbonContextualTabGroup();
                tabGroup.Header = header;

                ribbon.ContextualGroups.Add(tabGroup);
            }

            return tabGroup;
        }

        /// <summary>
        /// Ensures that a tab item with the specified header exists.
        /// </summary>
        /// <param name="ribbon">The ribbon.</param>
        /// <param name="header">The header.</param>
        /// <returns>The existing or newly created <see cref="RibbonTabItem"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="ribbon"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        public static RibbonTabItem EnsureTabItem(this Ribbon ribbon, string header)
        {
            Argument.IsNotNull("ribbon", ribbon);
            Argument.IsNotNullOrWhitespace("header", header);

            var tabItem = (from tab in ribbon.Tabs
                           where string.Equals(tab.Header.ToString(), header)
                           select tab).FirstOrDefault();
            if (tabItem == null)
            {
                tabItem = new RibbonTabItem();
                tabItem.Header = header;

                ribbon.Tabs.Add(tabItem);
            }

            return tabItem;
        }

        /// <summary>
        /// Ensures that a contextual tab item with the specified header exists.
        /// </summary>
        /// <param name="ribbon">The ribbon.</param>
        /// <param name="header">The header.</param>
        /// <param name="contextualTabGroupHeader">The contextual tab group header.</param>
        /// <returns>The existing or newly created <see cref="RibbonTabItem"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="ribbon"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref header="contextualTabGroupHeader"/> is <c>null</c> or whitespace.</exception>
        public static RibbonTabItem EnsureContextualTabItem(this Ribbon ribbon, string header, string contextualTabGroupHeader)
        {
            Argument.IsNotNull("ribbon", ribbon);
            Argument.IsNotNullOrWhitespace("header", header);
            Argument.IsNotNullOrWhitespace("contextualTabGroupHeader", contextualTabGroupHeader);

            var tabItem = EnsureTabItem(ribbon, header);
            tabItem.Group = ribbon.EnsureContextualTabGroup(contextualTabGroupHeader);

            return tabItem;
        }

        /// <summary>
        /// Ensures that a groupbox with the specified header exists.
        /// </summary>
        /// <param name="tabItem">The tab item.</param>
        /// <param name="header">The header.</param>
        /// <returns>The existing or newlhy created <see cref="RibbonGroupBox"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="tabItem"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        public static RibbonGroupBox EnsureGroupBox(this RibbonTabItem tabItem, string header)
        {
            Argument.IsNotNull("tabItem", tabItem);
            Argument.IsNotNullOrWhitespace("header", header);

            var groupBox = (from ribbonGroup in tabItem.Groups
                            where string.Equals(ribbonGroup.Header, header)
                            select ribbonGroup).FirstOrDefault();
            if (groupBox == null)
            {
                groupBox = new RibbonGroupBox();
                groupBox.Header = header;

                tabItem.Groups.Add(groupBox);
            }

            return groupBox;
        }

        /// <summary>
        /// Adds the ribbon item to the specified <see cref="ItemsControl"/>.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="ribbonItem">The ribbon item.</param>
        public static void AddRibbonItem(this ItemsControl itemsControl, IRibbonItem ribbonItem)
        {
            var ribbonControl = ribbonItem as IRibbonControl;

            if (ribbonControl != null)
            {
                Control ribbonItemControl = null;

                var ribbonButton = ribbonControl as IRibbonButton;
                if (ribbonButton != null)
                {
                    var ribbonSplitButton = ribbonButton as IRibbonSplitButton;

                    if (ribbonSplitButton != null)
                    {
                        if (ribbonButton.Command != null)
                        {
                            ribbonItemControl = itemsControl.AddSplitButton(ribbonSplitButton.Items, ribbonButton.ItemHeader, ribbonButton.ItemImage, ribbonButton.ItemImage, ribbonButton.Command);
                        }
                        else
                        {
                            ribbonItemControl = itemsControl.AddSplitButton(ribbonSplitButton.Items, ribbonButton.ItemHeader, ribbonButton.ItemImage, ribbonButton.ItemImage, ribbonButton.CommandName);
                        }
                    }
                    else
                    {
                        if (ribbonButton.Command != null)
                        {
                            ribbonItemControl = itemsControl.AddButton(ribbonButton.ItemHeader, ribbonButton.ItemImage, ribbonButton.ItemImage, ribbonButton.Command);
                        }
                        else
                        {
                            ribbonItemControl = itemsControl.AddButton(ribbonButton.ItemHeader, ribbonButton.ItemImage, ribbonButton.ItemImage, ribbonButton.CommandName);
                        }
                    }
                }

                var ribbonComboBox = ribbonControl as IRibbonComboBox;

                if (ribbonComboBox != null)
                {
                    ribbonItemControl = itemsControl.AddComboBox(ribbonComboBox.ItemHeader, ribbonComboBox.ItemsSource, ribbonComboBox.SelectedItem);
                }

                var ribbonContentControl = ribbonControl as IRibbonContentControl;

                if (ribbonContentControl != null)
                {
                    ribbonItemControl = itemsControl.AddContentControl(ribbonControl.ItemHeader, ribbonContentControl.ContentTemplate);
                }

                if (ribbonItemControl != null)
                {
                    if (ribbonControl.Layout != null && itemsControl is RibbonGroupBox)
                    {
                        ((RibbonGroupBox)itemsControl).ApplyLayout(ribbonItemControl, ribbonControl.Layout);
                    }

                    if (ribbonControl.Style != null)
                    {
                        ribbonItemControl.Style = ribbonControl.Style;
                    }

                    if (ribbonControl.ToolTip != null)
                    {
                        ribbonItemControl.ToolTip = new ScreenTip
                        {
                            Title = ribbonControl.ToolTip.Title,
                            Text = ribbonControl.ToolTip.Text,
                            Width = ribbonControl.ToolTip.Width,
                        };
                    }
                }
            }

            var ribbonItemsCollection = ribbonItem as IRibbonItemsCollection;

            if (ribbonItemsCollection != null)
            {
                var ribbonGallery = ribbonItemsCollection as IRibbonGallery;

                if (ribbonGallery != null)
                {
                    itemsControl.AddGallery(ribbonGallery.Items, ribbonGallery.Orientation, ribbonGallery.Selectable,
                        ribbonGallery.ItemWidth, ribbonGallery.ItemHeight, ribbonGallery.MinItemsInRow, ribbonGallery.MaxItemsInRow, ribbonGallery.ItemContainerStyle);
                }
            }
        }

        #region AddButton Methods

        /// <summary>
        /// Adds a new button to the specified <see cref="ItemsControl"/>.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <param name="command">The command.</param>
        /// <returns>The created button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref header="command"/> is <c>null</c>.</exception>
        public static Button AddButton(this ItemsControl itemsControl, string header, string icon, string largeIcon, ICommand command)
        {
            Argument.IsNotNull("command", command);

            var button = CreateButtonWithoutCommandBinding(itemsControl, header, icon, largeIcon);
            button.Command = command;

            return button;
        }

        /// <summary>
        /// Adds a new button to the specified <see cref="ItemsControl"/>.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <param name="commandName">The command name.</param>
        /// <param name="commandSource">The command source, can be <c>null</c> to respect the data context.</param>
        /// <returns>The created button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref header="commandName"/> is <c>null</c> or whitespace.</exception>
        public static Button AddButton(this ItemsControl itemsControl, string header, string icon, string largeIcon, string commandName, object commandSource = null)
        {
            Argument.IsNotNullOrWhitespace("commandName", commandName);

            var button = CreateButtonWithoutCommandBinding(itemsControl, header, icon, largeIcon);

            var commandBinding = new Binding(commandName);
            if (commandSource != null)
            {
                commandBinding.Source = commandSource;
            }

            button.SetBinding(ButtonBase.CommandProperty, commandBinding);

            return button;
        }

        /// <summary>
        /// Creates the button without command binding.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <returns>Button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        private static Button CreateButtonWithoutCommandBinding(this ItemsControl itemsControl, string header, string icon, string largeIcon)
        {
            Argument.IsNotNull("itemsControl", itemsControl);
            Argument.IsNotNullOrWhitespace("header", header);

            var button = new Button();
            button.Header = header;
            button.Icon = icon;
            button.LargeIcon = largeIcon;

            itemsControl.Items.Add(button);

            return button;
        }

        #endregion

        #region AddGallery Methods
        /// <summary>
        /// Adds a new gallery to the specified <see cref="ItemsControl" />.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="items">The list of items.</param>
        /// <param name="orientation">The gallery orientation.</param>
        /// <param name="selectable">if set to <c>true</c> is selectable.</param>
        /// <param name="itemWidth">Width of the item.</param>
        /// <param name="itemHeight">Height of the item.</param>
        /// <param name="minItemsInRow">The min items in row.</param>
        /// <param name="maxItemsInRow">The max items in row.</param>
        /// <param name="itemContainerStyle">The item container style.</param>
        /// <returns>
        /// The created gallery.
        /// </returns>
        public static Gallery AddGallery(this ItemsControl itemsControl, IEnumerable<IRibbonItem> items, Orientation orientation, bool selectable,
            double itemWidth = double.NaN, double itemHeight = double.NaN, int minItemsInRow = 2, int maxItemsInRow = 16, Style itemContainerStyle = null)
        {
            Argument.IsNotNull("itemsControl", itemsControl);
            Argument.IsNotNull("items", items);

            var gallery = new Gallery
            {
                Orientation = orientation,
                Selectable = selectable,
                MinItemsInRow = minItemsInRow,
                MaxItemsInRow = maxItemsInRow,
                ItemWidth = itemWidth,
                ItemHeight = itemHeight,
                //ItemContainerStyle = itemContainerStyle,
            };

            foreach (var ribbonItem in items)
            {
                gallery.AddRibbonItem(ribbonItem);
            }

            itemsControl.Items.Add(gallery);

            return gallery;
        }

        #endregion

        #region AddSplitButton Methods

        /// <summary>
        /// Adds a new button to the specified <see cref="ItemsControl"/>.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="items">The nested items.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <param name="command">The command.</param>
        /// <returns>The created split button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref header="command"/> is <c>null</c>.</exception>
        public static SplitButton AddSplitButton(this ItemsControl itemsControl, List<IRibbonItem> items, string header, string icon, string largeIcon, ICommand command)
        {
            Argument.IsNotNull("command", command);

            var button = CreateSplitButtonWithoutCommandBinding(itemsControl, items, header, icon, largeIcon);
            button.Command = command;

            return button;
        }

        /// <summary>
        /// Adds a new button to the specified <see cref="ItemsControl"/>.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="items">The nested items.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <param name="commandName">The command name.</param>
        /// <param name="commandSource">The command source, can be <c>null</c> to respect the data context.</param>
        /// <returns>The created split button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref header="commandName"/> is <c>null</c> or whitespace.</exception>
        public static SplitButton AddSplitButton(this ItemsControl itemsControl, List<IRibbonItem> items, string header, string icon, string largeIcon, string commandName, object commandSource = null)
        {
            Argument.IsNotNullOrWhitespace("commandName", commandName);

            var button = CreateSplitButtonWithoutCommandBinding(itemsControl, items, header, icon, largeIcon);

            var commandBinding = new Binding(commandName);
            if (commandSource != null)
            {
                commandBinding.Source = commandSource;
            }

            button.SetBinding(ButtonBase.CommandProperty, commandBinding);

            return button;
        }

        /// <summary>
        /// Creates the button without command binding.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="items">The nested items.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <returns>Split button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        private static SplitButton CreateSplitButtonWithoutCommandBinding(this ItemsControl itemsControl, List<IRibbonItem> items, string header, string icon, string largeIcon)
        {
            Argument.IsNotNull("itemsControl", itemsControl);
            Argument.IsNotNullOrWhitespace("header", header);

            var button = new SplitButton();
            button.Header = header;
            button.Icon = icon;
            button.LargeIcon = largeIcon;

            foreach (var ribbonItem in items)
            {
                button.AddRibbonItem(ribbonItem);
            }

            itemsControl.Items.Add(button);

            return button;
        }

        #endregion

        #region AddComboBox Methods

        /// <summary>
        /// Adds a new combobox to the specified <see cref="ItemsControl" />.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="header">The header.</param>
        /// <param name="itemsSource">The items source collection.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <param name="bindingSource">The binding source.</param>
        /// <returns>
        /// The created <see cref="ComboBox"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header" /> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl" /> is <c>null</c>.</exception>
        public static ComboBox AddComboBox(this ItemsControl itemsControl, string header, string itemsSource, string selectedItem, object bindingSource = null)
        {
            var comboBox = CreateComboBoxWithoutBinding(itemsControl, header);

            if (!string.IsNullOrWhiteSpace(itemsSource))
            {
                var itemsSourceBinding = new Binding(itemsSource);

                if (bindingSource != null)
                {
                    itemsSourceBinding.Source = bindingSource;
                }

                comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
            }

            if (!string.IsNullOrWhiteSpace(selectedItem))
            {
                var selectedItemBinding = new Binding(selectedItem);

                if (bindingSource != null)
                {
                    selectedItemBinding.Source = bindingSource;
                }

                comboBox.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);
            }

            return comboBox;
        }

        /// <summary>
        /// Adds the content control.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="header">The header.</param>
        /// <param name="template">The DataTemplate for the ContentTemplate of the ContentControl.</param>
        public static ContentControl AddContentControl(this ItemsControl itemsControl, string header, DataTemplate template)
        {
            var contentControl = new ContentControl { ContentTemplate = template };
            itemsControl.Items.Add(contentControl);

            var binding = new Binding("DataContext") { Source = contentControl };
            contentControl.SetBinding(ContentControl.ContentProperty, binding);

            return contentControl;
        }


        /// <summary>
        /// Creates the combobox without command binding.
        /// </summary>
        /// <param name="itemsControl">The items control.</param>
        /// <param name="header">The header.</param>
        /// <returns>
        /// Button.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref header="itemsControl" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header" /> is <c>null</c> or whitespace.</exception>
        private static ComboBox CreateComboBoxWithoutBinding(this ItemsControl itemsControl, string header)
        {
            Argument.IsNotNull("itemsControl", itemsControl);

            var comboBox = new ComboBox
            {
                Header = header,
            };

            itemsControl.Items.Add(comboBox);

            return comboBox;
        }

        #endregion

        #region ApplyLayout Methods

        /// <summary>
        /// Applies the layout.
        /// </summary>
        /// <param name="groupBox">The group box.</param>
        /// <param name="control">The control.</param>
        /// <param name="layout">The layout.</param>
        public static void ApplyLayout(this RibbonGroupBox groupBox, Control control, IRibbonItemLayout layout)
        {
            if (layout.Width > 0)
            {
                control.Width = layout.Width;
            }
        }


        #endregion


        /// <summary>
        /// Removes the specified <see cref="IRibbonControl"/> from the ribbon.
        /// </summary>
        /// <param name="ribbon">The ribbon.</param>
        /// <param name="ribbonControl">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbon"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonControl"/> is <c>null</c>.</exception>
        public static void RemoveItem(this Ribbon ribbon, IRibbonControl ribbonControl)
        {
            Argument.IsNotNull("ribbon", ribbon);
            Argument.IsNotNull("ribbonControl", ribbonControl);

            var ribbonTab = (from tab in ribbon.Tabs
                             where string.Equals(tab.Header.ToString(), ribbonControl.TabItemHeader)
                             select tab).FirstOrDefault();
            if (ribbonTab == null)
            {
                Log.Warning("Cannot find tab '{0}' on the ribbon, cannot remove item '{1}'", ribbonControl.TabItemHeader, ribbonControl.ItemHeader);
                return;
            }

            var ribbonGroupBox = (from groupBox in ribbonTab.Groups
                                  where string.Equals(groupBox.Header, ribbonControl.GroupBoxHeader)
                                  select groupBox).FirstOrDefault();
            if (ribbonGroupBox == null)
            {
                Log.Warning("Cannot find group '{0}' on the ribbon, cannot remove item '{1}'", ribbonControl.GroupBoxHeader, ribbonControl.ItemHeader);
                return;
            }

            var ribbonButton = (from button in ribbonGroupBox.Items.Cast<Button>()
                                where string.Equals(button.Header.ToString(), ribbonControl.ItemHeader)
                                select button).FirstOrDefault();
            if (ribbonButton == null)
            {
                Log.Warning("Cannot find group '{0}' on the ribbon, cannot remove item '{1}'", ribbonControl.GroupBoxHeader, ribbonControl.ItemHeader);
                return;
            }

            ribbonGroupBox.Items.Remove(ribbonButton);
        }
    }
}