using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;

namespace TableView
{
  public class TableViewColumn : ContentControl
  {
    public TableView ParentTableView { get; internal set; }
    public int ColumnIndex { get { return (ParentTableView == null) ? -1 : ParentTableView.Columns.IndexOf(this); } }
    
    #region Dependency Properties

    public enum ColumnSortDirection { None, Up, Down };
    public static readonly DependencyProperty SortDirectionProperty =
      DependencyProperty.Register("SortDirection", typeof(ColumnSortDirection), typeof(TableViewColumn), new FrameworkPropertyMetadata(ColumnSortDirection.None, new PropertyChangedCallback(OnSortDirectionPropertyChanged)));

    public ColumnSortDirection SortDirection
    {
      get { return (ColumnSortDirection)GetValue(SortDirectionProperty); }
      set { SetValue(SortDirectionProperty, value); }
    }

    private static void OnSortDirectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((TableViewColumn)d).NotifySortingChanged();
    }

    public static readonly DependencyProperty SortOrderProperty =
      DependencyProperty.Register("SortOrder", typeof(int), typeof(TableViewColumn), new FrameworkPropertyMetadata(0));

    public int SortOrder
    {
      get { return (int)GetValue(SortOrderProperty); }
      set { SetValue(SortOrderProperty, value); }
    }

    public static readonly DependencyProperty CellTemplateProperty = 
      DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(TableViewColumn), new FrameworkPropertyMetadata(null));

    public DataTemplate CellTemplate
    {
      get { return (DataTemplate)GetValue(CellTemplateProperty); }
      set { SetValue(CellTemplateProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty =
      DependencyProperty.Register("Title", typeof(object), typeof(TableViewColumn), new FrameworkPropertyMetadata(null));

    public object Title
    {
        get { return GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public static readonly DependencyProperty TitleTemplateProperty =
      DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(TableViewColumn), new FrameworkPropertyMetadata(null));

    public DataTemplate TitleTemplate
    {
      get { return (DataTemplate)GetValue(TitleTemplateProperty); }
      set { SetValue(TitleTemplateProperty, value); }
    }

    public static readonly DependencyProperty ContextBindingPathProperty =
      DependencyProperty.Register("ContextBindingPath", typeof(string), typeof(TableViewColumn), new FrameworkPropertyMetadata(null));

    public string ContextBindingPath
    {
      get { return (string)GetValue(ContextBindingPathProperty); }
      set { SetValue(ContextBindingPathProperty, value); }
    }
    #endregion

    public Binding WidthBinding { get; private set; }
    public Binding ContextBinding { get; set; }

    internal void NotifySortingChanged()
    {
      if (ParentTableView != null)
        ParentTableView.NotifySortingChanged(this);
    }

    internal void AdjustWidth(double width)
    {
      if (width < 0)
        width = 0;

      Width = width;  // adjust the width of this control

      if (ParentTableView != null)
        ParentTableView.NotifyColumnWidthChanged(this); // let the table view know that this has changed
    }

    public void GenerateCellContent(TableViewCell cell)
    {
      cell.ContentTemplate = CellTemplate;
      cell.HorizontalContentAlignment = HorizontalContentAlignment;

      if (ContextBinding == null && ContextBindingPath != null)
        ContextBinding = new Binding(ContextBindingPath);

      if (ContextBinding != null)
        BindingOperations.SetBinding(cell, DataContextProperty, ContextBinding);
    }

    public void FocusColumn()
    {
      if(ParentTableView != null )
        ParentTableView.FocusedColumnChanged(this);
    }

    public TableViewColumn()
      : base()
    {
      Width = 100;
      HorizontalContentAlignment = HorizontalAlignment.Stretch;

      WidthBinding = new Binding("Width");
      WidthBinding.Mode = BindingMode.OneWay;
      WidthBinding.Source = this;
    }
  }
}
