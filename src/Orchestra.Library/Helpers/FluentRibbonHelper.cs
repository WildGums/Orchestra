// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentRibbonHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
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

        #region AddButton Methods

        /// <summary>
        /// Adds a new button to the specified <see cref="RibbonGroupBox"/>.
        /// </summary>
        /// <param name="groupBox">The group box.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <param name="command">The command.</param>
        /// <returns>The created button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="groupBox"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref header="command"/> is <c>null</c>.</exception>
        public static Button AddButton(this RibbonGroupBox groupBox, string header, string icon, string largeIcon, ICommand command)
        {
            Argument.IsNotNull("command", command);

            var button = CreateButtonWithoutCommandBinding(groupBox, header, icon, largeIcon);
            button.Command = command;

            return button;
        }

        /// <summary>
        /// Adds a new button to the specified <see cref="RibbonGroupBox"/>.
        /// </summary>
        /// <param name="groupBox">The group box.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <param name="commandName">The command name.</param>
        /// <param name="commandSource">The command source, can be <c>null</c> to respect the data context.</param>
        /// <returns>The created button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="groupBox"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentException">The <paramref header="commandName"/> is <c>null</c> or whitespace.</exception>
        public static Button AddButton(this RibbonGroupBox groupBox, string header, string icon, string largeIcon, string commandName, object commandSource = null)
        {
            Argument.IsNotNullOrWhitespace("commandName", commandName);

            var button = CreateButtonWithoutCommandBinding(groupBox, header, icon, largeIcon);

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
        /// <param name="groupBox">The group box.</param>
        /// <param name="header">The header.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="largeIcon">The large icon.</param>
        /// <returns>Button.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="groupBox"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        private static Button CreateButtonWithoutCommandBinding(this RibbonGroupBox groupBox, string header, string icon, string largeIcon)
        {
            Argument.IsNotNull("groupBox", groupBox);
            Argument.IsNotNullOrWhitespace("header", header);

            var button = new Button();
            button.Header = header;
            button.Icon = icon;
            button.LargeIcon = largeIcon;

            groupBox.Items.Add(button);

            return button;
        }

        #endregion

        #region AddComboBox Methods

        /// <summary>
        /// Adds a new combobox to the specified <see cref="RibbonGroupBox" />.
        /// </summary>
        /// <param name="groupBox">The group box.</param>
        /// <param name="header">The header.</param>
        /// <param name="itemsSource">The items source collection.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <param name="bindingSource">The binding source.</param>
        /// <returns>
        /// The created <see cref="ComboBox"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref header="groupBox" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header" /> is <c>null</c> or whitespace.</exception>
        /// <exception cref="ArgumentNullException">The <paramref header="groupBox" /> is <c>null</c>.</exception>
        public static ComboBox AddComboBox(this RibbonGroupBox groupBox, string header, string itemsSource, string selectedItem, object bindingSource = null)
        {
            Argument.IsNotNull("itemsSource", itemsSource);
            Argument.IsNotNull("selectedItem", selectedItem);

            var comboBox = CreateComboBoxWithoutBinding(groupBox, header);

            var itemsSourceBinding = new Binding(itemsSource);
            var selectedItemBinding = new Binding(selectedItem);
            if (bindingSource != null)
            {
                itemsSourceBinding.Source = bindingSource;
                selectedItemBinding.Source = bindingSource;
            }

            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, itemsSourceBinding);
            comboBox.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);

            return comboBox;
        }

        /// <summary>
        /// Adds the content control.
        /// </summary>
        /// <param name="groupBox">The group box.</param>
        /// <param name="header">The header.</param>
        /// <param name="template">The DataTemplate for the ContentTemplate of the ContentControl.</param>
        public static void AddContentControl(this RibbonGroupBox groupBox, string header, DataTemplate template)
        {
            var contentControl = new ContentControl { ContentTemplate = template };
            groupBox.Items.Add(contentControl);
        }


        /// <summary>
        /// Creates the combobox without command binding.
        /// </summary>
        /// <param name="groupBox">The group box.</param>
        /// <param name="header">The header.</param>
        /// <param name="isEditable">if set to <c>true</c> the combo box is editable.</param>
        /// <returns>
        /// Button.
        /// </returns>
        /// <exception cref="ArgumentNullException">The <paramref header="groupBox" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header" /> is <c>null</c> or whitespace.</exception>
        private static ComboBox CreateComboBoxWithoutBinding(this RibbonGroupBox groupBox, string header, bool isEditable = false)
        {
            Argument.IsNotNull("groupBox", groupBox);

            var comboBox = new ComboBox
            {
                Header = header,
                IsEditable = isEditable
            };

            groupBox.Items.Add(comboBox);

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
        /// Removes the specified <see cref="IRibbonItem"/> from the ribbon.
        /// </summary>
        /// <param name="ribbon">The ribbon.</param>
        /// <param name="ribbonItem">The ribbon item.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbon"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="ribbonItem"/> is <c>null</c>.</exception>
        public static void RemoveItem(this Ribbon ribbon, IRibbonItem ribbonItem)
        {
            Argument.IsNotNull("ribbon", ribbon);
            Argument.IsNotNull("ribbonItem", ribbonItem);

            var ribbonTab = (from tab in ribbon.Tabs
                             where string.Equals(tab.Header.ToString(), ribbonItem.TabItemHeader)
                             select tab).FirstOrDefault();
            if (ribbonTab == null)
            {
                Log.Warning("Cannot find tab '{0}' on the ribbon, cannot remove item '{1}'", ribbonItem.TabItemHeader, ribbonItem.ItemHeader);
                return;
            }

            var ribbonGroupBox = (from groupBox in ribbonTab.Groups
                                  where string.Equals(groupBox.Header, ribbonItem.GroupBoxHeader)
                                  select groupBox).FirstOrDefault();
            if (ribbonGroupBox == null)
            {
                Log.Warning("Cannot find group '{0}' on the ribbon, cannot remove item '{1}'", ribbonItem.GroupBoxHeader, ribbonItem.ItemHeader);
                return;
            }

            var ribbonButton = (from button in ribbonGroupBox.Items.Cast<Button>()
                                where string.Equals(button.Header.ToString(), ribbonItem.ItemHeader)
                                select button).FirstOrDefault();
            if (ribbonButton == null)
            {
                Log.Warning("Cannot find group '{0}' on the ribbon, cannot remove item '{1}'", ribbonItem.GroupBoxHeader, ribbonItem.ItemHeader);
                return;
            }

            ribbonGroupBox.Items.Remove(ribbonButton);
        }
    }
}