using System.Windows.Controls;
using System.Windows;

namespace TableView
{
  public class TableViewHeaderPresenter : ItemsControl
  {
    public Panel HeaderItemsPanel { get; set; }
    
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

    internal TableViewColumnHeader GetColumnHeaderAtLocation(Point loc)
    {
      var uie = InputHitTest(loc) as FrameworkElement;
      if (uie != null)
      {
        return TableViewUtils.FindParent<TableViewColumnHeader>(uie);
      }
      return null;
    }

    internal void HeaderInvalidateArrange()
    {
      if (HeaderItemsPanel != null)
        HeaderItemsPanel.InvalidateArrange();
    }

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      base.PrepareContainerForItemOverride(element, item);

      (element as TableViewColumnHeader).Width = (item as TableViewColumn).Width;
    }

    protected override DependencyObject GetContainerForItemOverride()
    {
      return new TableViewColumnHeader();
    }

    protected override bool IsItemItsOwnContainerOverride(object item)
    {
      return (item is TableViewColumnHeader);
    }
  }
}
