using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TableView
{
  //-----------------------------------------------------------------------------------
  public class TableViewHeaderThumb : Thumb
  {
    public TableViewHeaderThumb()
      : base()
    {
      PreviewMouseLeftButtonDown += (s, e) => Mouse.Capture(this);
      PreviewMouseLeftButtonUp += (s, e) => Mouse.Capture(null);
      DragDelta += onDragDelta;
    }

    public void onDragDelta(object sender, DragDeltaEventArgs e)
    {
      var tvch = TableViewUtils.GetAncestorByType<TableViewColumnHeader>(this);

      if (tvch != null)
      {
        var width = tvch.Width + e.HorizontalChange;
        tvch.AdjustWidth(width);
      }
    }
  }

}
