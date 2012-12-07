using System.Windows.Controls;
using System.Windows.Input;

namespace TableView
{
  public interface ITableViewColumnHeader
  {
    int ColumnIndex { get; }
    TableViewColumn Column { get; }
  }

  public class TableViewColumnHeader : ContentControl, ITableViewColumnHeader
  {
    public int ColumnIndex { get { return Column.ColumnIndex; } }
    public TableViewColumn Column { get { return this.Content as TableViewColumn; } }

    internal void AdjustWidth(double width)
    {
      if (width < 1)
        width = 1;

      Width = width;  // adjust the width of this control

      Column.AdjustWidth(width);  // adjust the width of the column
    }

    public override void OnApplyTemplate()
    {
      var col = this.Content as TableViewColumn;
      if (col != null)
        this.ContentTemplate = col.TitleTemplate;

      base.OnApplyTemplate();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      Focus();
      Column.FocusColumn();
      base.OnMouseLeftButtonDown(e);
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
      Focus();
      Column.FocusColumn();
      base.OnMouseRightButtonDown(e);
    }
  }
}
