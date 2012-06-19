// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FluentRibbonHelper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Catel;
    using Catel.Logging;
    using Fluent;
    using Models;

    /// <summary>
    /// Helper class for the <see cref="Ribbon"/> control.
    /// </summary>
    public static class FluentRibbonHelper
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Ensures that a tab item with the specified header exists.
        /// </summary>
        /// <param header="ribbon">The ribbon.</param>
        /// <param header="header">The header.</param>
        /// <returns>The existing or newly created <see cref="RibbonTabItem"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="ribbon"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        public static RibbonTabItem EnsureTabItem(this Ribbon ribbon, string header)
        {
            Argument.IsNotNull("ribbon", ribbon);
            Argument.IsNotNullOrWhitespace("header", header);

            var tabItem = (from tab in ribbon.Tabs
                           where string.Compare(tab.Header.ToString(), header) == 0
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
        /// <param header="tabItem">The tab item.</param>
        /// <param header="header">The header.</param>
        /// <returns>The existing or newlhy created <see cref="RibbonGroupBox"/>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref header="tabItem"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref header="header"/> is <c>null</c> or whitespace.</exception>
        public static RibbonGroupBox EnsureGroupBox(this RibbonTabItem tabItem, string header)
        {
            Argument.IsNotNull("tabItem", tabItem);
            Argument.IsNotNullOrWhitespace("header", header);

            var groupBox = (from ribbonGroup in tabItem.Groups
                            where string.Compare(ribbonGroup.Header, header) == 0
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
            Argument.IsNotNull("groupBox", groupBox);
            Argument.IsNotNullOrWhitespace("header", header);
            Argument.IsNotNull("command", command);

            var button = new Button();
            button.Header = header;
            button.Icon = icon;
            button.LargeIcon = largeIcon;
            button.Command = command;

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
                             where tab.Header.ToString() == ribbonItem.TabItemHeader
                             select tab).FirstOrDefault();
            if (ribbonTab == null)
            {
                Log.Warning("Cannot find tab '{0}' on the ribbon, cannot remove item '{1}'", ribbonItem.TabItemHeader, ribbonItem.ItemHeader);
                return;
            }

            var ribbonGroupBox = (from groupBox in ribbonTab.Groups
                                  where groupBox.Header == ribbonItem.GroupBoxHeader
                                  select groupBox).FirstOrDefault();
            if (ribbonGroupBox == null)
            {
                Log.Warning("Cannot find group '{0}' on the ribbon, cannot remove item '{1}'", ribbonItem.GroupBoxHeader, ribbonItem.ItemHeader);
                return;
            }

            var ribbonButton = (from button in ribbonGroupBox.Items.Cast<Button>()
                                where button.Header.ToString() == ribbonItem.ItemHeader
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