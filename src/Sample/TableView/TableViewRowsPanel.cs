// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableViewRowsPanel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TableView
{
    using System.Windows;
    using System.Windows.Controls;

    public class TableViewRowsPanel : VirtualizingStackPanel
    {
        private TableViewRowsPresenter _parentRowsPresenter;

        private TableView _parentTableView;

        private TableView ParentTableView
        {
            get { return _parentTableView ?? (_parentTableView = TableViewUtils.FindParent<TableView>(this)); }
        }

        private TableViewRowsPresenter ParentRowsPresenter
        {
            get { return _parentRowsPresenter ?? (_parentRowsPresenter = TableViewUtils.FindParent<TableViewRowsPresenter>(this)); }
        }

        protected override void OnViewportOffsetChanged(Vector oldViewportOffset, Vector newViewportOffset)
        {
            ParentTableView.HorizontalScrollOffset = newViewportOffset.X;
        }

        protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
        {
            base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
            Style = ParentTableView.RowsPanelStyle;
            ParentRowsPresenter.RowsPanel = this;
        }

        public void BringRowIntoView(int idx)
        {
            if (idx >= 0 && idx < ParentRowsPresenter.Items.Count)
            {
                BringIndexIntoView(idx);
            }
        }

        internal void ColumnsChanged()
        {
            foreach (object child in Children)
            {
                var tableViewCellsPresenter = child as TableViewCellsPresenter;
                if (tableViewCellsPresenter != null)
                {
                    tableViewCellsPresenter.ColumnsChanged();
                }
            }
        }

        internal void RowsInvalidateArrange()
        {
            foreach (object child in Children)
            {
                var tableViewCellsPresenter = child as TableViewCellsPresenter;
                if (tableViewCellsPresenter != null)
                {
                    tableViewCellsPresenter.CellsInvalidateArrange();
                }
            }
        }
    }
}