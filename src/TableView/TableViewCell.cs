using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace TableView
{
  //------------------------------------------------------------------
  public class TableViewCell : ContentControl
  {
    public static readonly DependencyPropertyKey IsSelectedPropertyKey =
          DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(TableViewCell), new PropertyMetadata(false, OnIsSelectedChanged));

    public static readonly DependencyProperty IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;

    private static void OnIsSelectedChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
    {
      if( (bool)e.NewValue )
        (source as TableViewCell).UpdateSelection();
    }

    private void UpdateSelection()
    {
      if (ParentTableView.SelectedCell != null)
        ParentTableView.SelectedCell.IsSelected = false;
      ParentTableView.SelectedCell = this;
    }

    public bool IsSelected
    {
      get { return (bool)GetValue(IsSelectedProperty); }
      private set { SetValue(IsSelectedPropertyKey, value); }
    }

    private TableViewCellsPresenter ParentCellsPresenter = null;
    private TableViewColumn _column = null;

    public TableView ParentTableView = null;
    public int ColumnIndex { get { return ParentTableView.Columns.IndexOf(_column); } }
    public object Item { get { return ParentCellsPresenter.Item; } }

    protected override Size ArrangeOverride(Size arrangeBounds)
    {
      var tmp = new Size(Math.Max(0.0, arrangeBounds.Width - 1), arrangeBounds.Height);
      Size sz = base.ArrangeOverride(tmp);
      sz.Width += 1;
      return sz;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      var tmp = new Size(Math.Max(0.0, constraint.Width - 1), constraint.Height);
      Size sz = base.MeasureOverride(tmp);
      sz.Width += 1;
      return sz;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
      base.OnRender(drawingContext);

      double verticalGridLineThickness = 1;
      Rect rectangle = new Rect(new Size(verticalGridLineThickness, base.RenderSize.Height));
      rectangle.X = base.RenderSize.Width - verticalGridLineThickness;
      drawingContext.DrawRectangle(ParentTableView.GridLinesBrush, null, rectangle);
    }

    public void PrepareCell(TableViewCellsPresenter parent, int idx)
    {
      ParentCellsPresenter = parent;
      ParentTableView = parent.ParentTableView;

      var column = ParentTableView.Columns[idx];

      //IsSelected = ParentCellsPresenter.IsSelected() && (ParentTableView.FocusedColumnIndex == column.ColumnIndex);

      if (_column != column)
      {
        _column = column;
        this.Width = column.Width;
        BindingOperations.ClearBinding(this, WidthProperty);
        BindingOperations.SetBinding(this, WidthProperty, column.WidthBinding);
        Focusable = ParentTableView.CellNavigation;
      }
      column.GenerateCellContent(this);
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
      base.OnGotFocus(e);
      if (ParentTableView.CellNavigation)
        _column.FocusColumn();
      IsSelected = true;
        //ParentTableView.FocusedItemChanged(ParentCellsPresenter);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseLeftButtonDown(e);
      _column.FocusColumn();
      Focus();
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
      base.OnMouseRightButtonDown(e);
      _column.FocusColumn();
      Focus();
    }

  }
}
