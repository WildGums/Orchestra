using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Media;

namespace TableView
{
  public class TableViewCellsPanel : Panel
  {
    private TableView ParentTableView;

    protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
    {
      base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);

      var rowPresenter = TableViewUtils.FindParent<TableViewCellsPresenter>(this);

      if (rowPresenter != null)
      {
        rowPresenter.CellsPanel = this;
        ParentTableView = rowPresenter.ParentTableView;
        this.Style = ParentTableView.CellsPanelStyle;
      }
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
      if (ParentTableView != null)
      {
        var columns = ParentTableView.Columns;
        var children = base.Children;
        double leftX = ParentTableView.HorizontalScrollOffset;
        int fixedColumnCount = ParentTableView.FixedColumnCount;

        Rect fixedClip = ParentTableView.FixedClipRect;
        fixedClip.Height = arrangeSize.Height;

        // Arrange the children into a line
        int idx = 0;
        Rect cellRect = new Rect(0, 0, 0, arrangeSize.Height);
        foreach (var child in children)
        {
          if (idx == fixedColumnCount)
            leftX -= ParentTableView.HorizontalScrollOffset;

          cellRect.X = leftX;
          cellRect.Width = columns[idx].Width;
          leftX += cellRect.Width;

          (child as UIElement).Clip = null;
          if (idx >= fixedColumnCount)
          {
            if (cellRect.Right < fixedClip.Right)
              cellRect.X = -cellRect.Width;   // hide children that are to the left of the fixed columns
            else
            {
              var overlap = fixedClip.Right - cellRect.X; // check for columns that overlap the fixed columns and clip them
              if (overlap > 0)
              {
                var r = new Rect(overlap, cellRect.Y, cellRect.Width - overlap, cellRect.Height);
                (child as UIElement).Clip = new RectangleGeometry(r);
              }
            }
          }
          (child as UIElement).Arrange(cellRect);

          ++idx;
        }
      }
      return arrangeSize;
    }

    Size size = Size.Empty;
    protected override Size MeasureOverride(Size availableSize)
    {
      size = new Size();

      var children = base.Children;
      foreach (var child in children)
      {
        var element = (child as UIElement);
        element.Measure(availableSize);
        size.Width += element.DesiredSize.Width;
        size.Height = Math.Max(size.Height, element.DesiredSize.Height);
      }

      // Set a default height for the row if not set by the children
      if (size.Height <= 5.0)
        size.Height = 15.96;
      return size;
    }
  }
}
