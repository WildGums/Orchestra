using System.Windows.Controls;
using System.Windows;

namespace TableView
{
  public class TableViewRowsPanel : VirtualizingStackPanel
  {
    private TableView _parentTableView;
    private TableView ParentTableView
    {
      get
      {
        if (_parentTableView == null)
          _parentTableView = TableViewUtils.FindParent<TableView>(this);
        return _parentTableView;
      }
    }

    private TableViewRowsPresenter _parentRowsPresenter;
    private TableViewRowsPresenter ParentRowsPresenter
    {
      get
      {
        if (_parentRowsPresenter == null)
          _parentRowsPresenter = TableViewUtils.FindParent<TableViewRowsPresenter>(this);
        return _parentRowsPresenter;
      }
    }

    protected override void OnViewportOffsetChanged(Vector oldViewportOffset, Vector newViewportOffset)
    {
      ParentTableView.HorizontalScrollOffset = newViewportOffset.X;
    }

    protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
    {
      base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);
      this.Style = ParentTableView.RowsPanelStyle;
      this.ParentRowsPresenter.RowsPanel = this;
    }

    public void BringRowIntoView(int idx)
    {
      if( idx >= 0 && idx < ParentRowsPresenter.Items.Count)
        this.BringIndexIntoView(idx);
    }

    internal void ColumnsChanged()
    {
      foreach (var child in Children)
        (child as TableViewCellsPresenter).ColumnsChanged();

    }

    internal void RowsInvalidateArrange()
    {
      foreach (var child in Children)
        (child as TableViewCellsPresenter).CellsInvalidateArrange();
    }
  }
}
