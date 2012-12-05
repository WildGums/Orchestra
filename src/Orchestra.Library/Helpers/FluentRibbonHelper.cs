// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentRibbonHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.Linq;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using Catel;
    using Catel.Logging;
    using Fluent;
    using Models;

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