using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Media;

namespace TableView
{
  public class TableViewHeaderPanel : Panel
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

    protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
    {
      base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);

      this.Style = ParentTableView.HeaderPanelStyle;

      ParentTableView.HeaderRowPresenter.HeaderItemsPanel = this;
    }

    protected override Size ArrangeOverride( Size arrangeSize)
    {
      var columns = ParentTableView.Columns;
      var children = base.Children;
      double leftX = 0;
      int fixedColumnCount = ParentTableView.FixedColumnCount;

      ParentTableView.ResetFixedClipRect();

      Rect fixedClip = ParentTableView.FixedClipRect;
      fixedClip.X = 0;
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
      return arrangeSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      var size = new Size();

      var children = base.Children;
      foreach (var child in children)
      {
        var element = (child as UIElement);
        element.Measure(availableSize);
        size.Width += element.DesiredSize.Width;
        size.Height = Math.Max(size.Height, element.DesiredSize.Height);
      }

      return size;
    }
  }
}
