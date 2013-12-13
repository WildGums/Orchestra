// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableViewCellsPresenter.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TableView
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public class TableViewCellsPresenter : ItemsControl
    {
        public static readonly DependencyPropertyKey IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsSelected", typeof(bool), typeof(TableViewCellsPresenter), new PropertyMetadata(false, OnIsSelectedChanged));

        public static readonly DependencyProperty IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            private set { SetValue(IsSelectedPropertyKey, value); }
        }

        public TableView ParentTableView { get; set; }

        public TableViewCellsPanel CellsPanel { get; set; }

        protected override bool HandlesScrolling
        {
            get { return true; }
        }

        public object Item
        {
            get { return ItemsSource == null ? null : (ItemsSource as TableViewCellCollection).CopyObject; }
            private set
            {
                if (ItemsSource == null)
                {
                    ItemsSource = new TableViewCellCollection(value, ParentTableView.Columns.Count);
                }
                else
                {
                    (ItemsSource as TableViewCellCollection).CopyObject = value;
                }
            }
        }

        private static void OnIsSelectedChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var tableViewCellsPresenter = source as TableViewCellsPresenter;
                if (tableViewCellsPresenter != null)
                {
                    tableViewCellsPresenter.UpdateSelection();
                }
            }
        }

        private void UpdateSelection()
        {
            if (ParentTableView.SelectedCellsPresenter != null)
            {
                ParentTableView.SelectedCellsPresenter.IsSelected = false;
            }
            ParentTableView.SelectedCellsPresenter = this;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ParentTableView.ShowHorizontalGridLines)
            {
                var pen = new Pen(ParentTableView.GridLinesBrush, 1.0);
                drawingContext.DrawLine(
                    pen, new Point(1, base.RenderSize.Height - 0.5), new Point(base.RenderSize.Width, base.RenderSize.Height - 0.5));
                //Rect rectangle = new Rect(new Size(base.RenderSize.Width, 1));
                //rectangle.Y = base.RenderSize.Height - 1;
                //drawingContext.DrawRectangle(ParentTableView.GridLinesBrush, null, rectangle);
            }
        }

        public void ColumnsChanged()
        {
            object item = Item;
            ItemsSource = null;
            Item = item;

            CellsInvalidateArrange();
        }

        public void UpdateColumns()
        {
            var tableViewCellCollection = ItemsSource as TableViewCellCollection;
            if (tableViewCellCollection != null)
            {
                tableViewCellCollection.Count = ParentTableView.Columns.Count;
            }
        }

        public void CellsInvalidateArrange()
        {
            UpdateColumns();

            if (CellsPanel != null)
            {
                CellsPanel.InvalidateArrange();
            }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            if (ParentTableView.ShowHorizontalGridLines)
            {
                var tmp = new Size(arrangeBounds.Width, arrangeBounds.Height - 1);
                Size size = base.ArrangeOverride(tmp);
                size.Height += 1;
                return size;
            }

            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (ParentTableView.ShowHorizontalGridLines)
            {
                Size tmp = constraint;

                if (tmp.Height > 0)
                {
                    tmp = new Size(constraint.Width, constraint.Height - 1);
                }

                Size size = base.MeasureOverride(tmp);
                size.Height += 1;
                return size;
            }
            return base.MeasureOverride(constraint);
        }

        public void PrepareRow(TableView parent, object dataItem)
        {
            ParentTableView = parent;

            Focusable = ParentTableView.CellNavigation == false;

            Item = dataItem;

            // set the selected state for this row
            TableViewCellsPresenter scp = ParentTableView.SelectedCellsPresenter;
            if (scp != null)
            {
                IsSelected = ParentTableView.IndexOfRow(scp) == ParentTableView.IndexOfRow(this);
            }
        }

        public void Clear()
        {
            Item = null;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var tableViewCell = element as TableViewCell;
            if (tableViewCell != null)
            {
                tableViewCell.PrepareCell(this, ItemContainerGenerator.IndexFromContainer(element));
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TableViewCell();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TableViewCell);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            Select();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            Select();
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);

            Select();
        }

        internal void Select()
        {
            ParentTableView.FocusedRowChanged(this);
            IsSelected = true;
        }
    }
}