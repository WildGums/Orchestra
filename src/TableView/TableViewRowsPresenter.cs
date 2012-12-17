// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableViewRowsPresenter.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TableView
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    public class TableViewRowsPresenter : ItemsControl
    {
        private int _focusedRowIndex;

        private TableView _parentTableView;

        public TableViewRowsPanel RowsPanel { get; set; }

        private TableView ParentTableView
        {
            get { return _parentTableView ?? (_parentTableView = TableViewUtils.FindParent<TableView>(this)); }
        }

        protected override bool HandlesScrolling
        {
            get { return true; }
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            BringIndexIntoView(ParentTableView.FocusedRowIndex);
        }

        internal object GetItemAtLocation(Point loc)
        {
            var uie = InputHitTest(loc) as FrameworkElement;

            if (uie != null)
            {
                var rowPresenter = TableViewUtils.GetAncestorByType<TableViewCellsPresenter>(uie);

                if (rowPresenter != null)
                {
                    return rowPresenter.Item;
                }
            }

            return null;
        }

        internal int GetCellIndexAtLocation(Point loc)
        {
            var uie = InputHitTest(loc) as FrameworkElement;
            if (uie != null)
            {
                var rowPresenter = TableViewUtils.GetAncestorByType<TableViewCellsPresenter>(uie);

                if (rowPresenter != null)
                {
                    var cell = TableViewUtils.FindParent<TableViewCell>(uie);
                    if (cell != null)
                    {
                        return rowPresenter.ItemContainerGenerator.IndexFromContainer(cell);
                    }
                }
            }
            return -1;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ItemsSource = ParentTableView.ItemsSource;
        }

        public void ColumnsChanged()
        {
            if (RowsPanel != null)
            {
                RowsPanel.ColumnsChanged();
            }
        }

        public void RowsInvalidateArrange()
        {
            if (RowsPanel != null)
            {
                RowsPanel.RowsInvalidateArrange();
            }
        }

        // Container generator
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            var tableViewCellsPresenter = element as TableViewCellsPresenter;
            if (tableViewCellsPresenter != null)
            {
                tableViewCellsPresenter.Clear();
            }
            base.ClearContainerForItemOverride(element, item);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            var tableViewCellsPresenter = element as TableViewCellsPresenter;
            if (tableViewCellsPresenter != null)
            {
                tableViewCellsPresenter.PrepareRow(ParentTableView, item);
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            var container = new TableViewCellsPresenter();
            return container;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TableViewCellsPresenter);
        }

        private void OnStatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                ItemContainerGenerator.StatusChanged -= OnStatusChanged;

                Select(_focusedRowIndex);
            }
        }

        private bool Select(int index)
        {
            var cellsPresenter = ItemContainerGenerator.ContainerFromIndex(index) as TableViewCellsPresenter;
            if (cellsPresenter != null)
            {
                cellsPresenter.Select();
                cellsPresenter.Focus();
                cellsPresenter.BringIntoView();
                return true;
            }
            return false;
        }

        internal void BringIndexIntoView(int idx)
        {
            if (RowsPanel == null || idx < 0 || idx >= Items.Count)
            {
                return;
            }

            _focusedRowIndex = idx;
            if (!Select(_focusedRowIndex))
            {
                ItemContainerGenerator.StatusChanged += OnStatusChanged;
                RowsPanel.BringRowIntoView(_focusedRowIndex);
            }
        }

        protected void OnPageUpOrDownKeyDown(KeyEventArgs e)
        {
            bool moveForward = e.Key == Key.Next;

            int idx = ItemContainerGenerator.IndexFromContainer(e.OriginalSource as FrameworkElement);

            int viewheight = (RowsPanel != null) ? (int)RowsPanel.ViewportHeight : 0;
            idx = moveForward ? idx + viewheight : idx - viewheight; // calculate the new index for the focus

            if (idx < 0 || idx >= Items.Count) // adjust to the bounds of the collection
            {
                idx = moveForward ? Items.Count - 1 : 0;
            }

            BringIndexIntoView(idx);
            e.Handled = true;
        }

        protected void OnHomeOrEndKeyDown(KeyEventArgs e)
        {
            int idx = (e.Key == Key.Home) ? 0 : Items.Count - 1;
            BringIndexIntoView(idx);
            e.Handled = true;
        }

        protected void OnLeftOrRightKeyDown(KeyEventArgs e)
        {
            if (ParentTableView.CellNavigation == false)
            {
                e.Handled = true;
            }
            else
            {
                if (e.Key == Key.Left && ParentTableView.FocusedColumnIndex <= 0)
                {
                    e.Handled = true;
                }
                if (e.Key == Key.Right && ParentTableView.FocusedColumnIndex >= ParentTableView.Columns.Count - 1)
                {
                    e.Handled = true;
                }
            }
        }

        protected void OnUpOrDownKeyDown(KeyEventArgs e)
        {
            int idx = ItemContainerGenerator.IndexFromContainer(e.OriginalSource as FrameworkElement);

            // make sure that we are within the collection
            if (e.Key == Key.Up && idx <= 0)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Down && idx >= Items.Count - 1)
            {
                e.Handled = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            Key key = e.Key;
            switch (key)
            {
                case Key.Prior:
                case Key.Next:
                    OnPageUpOrDownKeyDown(e);
                    break;

                case Key.End:
                case Key.Home:
                    OnHomeOrEndKeyDown(e);
                    break;

                case Key.Left:
                case Key.Right:
                    OnLeftOrRightKeyDown(e);
                    break;

                case Key.Up:
                case Key.Down:
                    OnUpOrDownKeyDown(e);
                    break;
            }

            if (!e.Handled)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            // if we have clicked on the control but not on a row.
            if (GetItemAtLocation(e.GetPosition(this)) == null)
            {
                Focus();
            }
            base.OnMouseDown(e);
        }
    }
}