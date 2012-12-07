// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TableView.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TableView
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    [TemplatePart(Name = "PART_HeaderPresenter", Type = typeof(TableViewHeaderPresenter))]
    [TemplatePart(Name = "PART_HeaderPanel", Type = typeof(Panel))]
    [TemplatePart(Name = "PART_RowsPresenter", Type = typeof(TableViewRowsPresenter))]
    [TemplatePart(Name = "PART_RowsPanel", Type = typeof(Panel))]
    public class TableView : Control
    {
        private ObservableCollection<TableViewColumn> _columns;

        private Rect _fixedClipRect = Rect.Empty;

        static TableView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TableView), new FrameworkPropertyMetadata(typeof(TableView)));
        }

        public TableView()
        {
            ResetFixedClipRect();

            Columns = new ObservableCollection<TableViewColumn>();
            //SnapsToDevicePixels = true;
            //UseLayoutRounding = true;
            //Columns.CollectionChanged += ColumnsChanged;
        }

        internal TableViewHeaderPresenter HeaderRowPresenter { get; set; }

        internal TableViewRowsPresenter RowsPresenter { get; set; }

        internal TableViewCellsPresenter SelectedCellsPresenter { get; set; }

        internal TableViewCell SelectedCell { get; set; }

        internal Rect FixedClipRect
        {
            get
            {
                if (_fixedClipRect == Rect.Empty)
                {
                    double width = 0.0;
                    if (Columns.Count >= FixedColumnCount)
                    {
                        for (int i = 0; i < FixedColumnCount; ++i)
                        {
                            width += Columns[i].Width;
                        }
                    }

                    _fixedClipRect = new Rect(HorizontalScrollOffset, 0, width, 0);
                }
                return _fixedClipRect;
            }
        }

        public double HeaderHeight
        {
            get { return HeaderRowPresenter == null ? 0 : HeaderRowPresenter.Height; }
        }

        public ItemCollection Items
        {
            get
            {
                if (RowsPresenter != null)
                {
                    return RowsPresenter.Items;
                }
                return null;
            }
        }

        public ObservableCollection<TableViewColumn> Columns
        {
            get { return _columns; }
            set
            {
                if (_columns != null)
                {
                    _columns.CollectionChanged -= ColumnsChanged;
                }

                _columns = value;

                if (_columns != null)
                {
                    _columns.CollectionChanged += ColumnsChanged;
                }
            }
        }

        public event EventHandler<TableViewColumnEventArgs> ColumnWidthChanged;

        public event EventHandler<TableViewColumnEventArgs> SortingChanged;

        internal void ResetFixedClipRect()
        {
            _fixedClipRect = Rect.Empty;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            HeaderRowPresenter = GetTemplateChild("PART_HeaderPresenter") as TableViewHeaderPresenter;
            RowsPresenter = GetTemplateChild("PART_RowsPresenter") as TableViewRowsPresenter;
        }

        internal void NotifyColumnWidthChanged(TableViewColumn column)
        {
            if (ColumnWidthChanged != null)
            {
                ColumnWidthChanged(this, new TableViewColumnEventArgs(column));
            }
        }

        internal void NotifySortingChanged(TableViewColumn column)
        {
            if (SortingChanged != null)
            {
                SortingChanged(this, new TableViewColumnEventArgs(column));
            }
        }

        private void ColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (TableViewColumn col in Columns)
            {
                col.ParentTableView = this;
            }

            if (HeaderRowPresenter != null)
            {
                ResetFixedClipRect();
                HeaderRowPresenter.HeaderInvalidateArrange();
            }

            if (RowsPresenter != null)
            {
                RowsPresenter.ColumnsChanged();
            }
        }

        private void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (HeaderRowPresenter != null)
            {
                ResetFixedClipRect();
                HeaderRowPresenter.HeaderInvalidateArrange();
            }

            if (RowsPresenter != null)
            {
                RowsPresenter.RowsInvalidateArrange();
            }
        }

        internal int IndexOfRow(TableViewCellsPresenter cp)
        {
            if (RowsPresenter != null)
            {
                return RowsPresenter.ItemContainerGenerator.IndexFromContainer(cp);
            }
            return -1;
        }

        internal void FocusedRowChanged(TableViewCellsPresenter cp)
        {
            FocusedRowIndex = IndexOfRow(cp);
            SelectedRowIndex = FocusedRowIndex;
        }

        internal void FocusedColumnChanged(TableViewColumn col)
        {
            FocusedColumnIndex = Columns.IndexOf(col);
            SelectedColumnIndex = FocusedColumnIndex;
        }

        public TableViewColumnHeader GetColumnHeaderAtLocation(Point loc)
        {
            if (HeaderRowPresenter != null)
            {
                return HeaderRowPresenter.GetColumnHeaderAtLocation(loc);
            }
            return null;
        }

        public TableViewColumn GetColumnAtLocation(Point loc)
        {
            TableViewColumnHeader ch = GetColumnHeaderAtLocation(loc);
            if (ch != null)
            {
                return Columns[ch.ColumnIndex];
            }

            return null;
        }

        public object GetItemAtLocation(Point loc)
        {
            loc.Y -= HeaderRowPresenter.RenderSize.Height;
            if (RowsPresenter != null)
            {
                return RowsPresenter.GetItemAtLocation(loc);
            }
            return null;
        }

        public int GetCellIndexAtLocation(Point loc)
        {
            loc.Y -= HeaderRowPresenter.RenderSize.Height;
            if (RowsPresenter != null)
            {
                return RowsPresenter.GetCellIndexAtLocation(loc);
            }
            return -1;
        }

        #region Dependency Properties

        #region CellNavigation dependency property
        public static readonly DependencyProperty CellNavigationProperty = DependencyProperty.Register(
            "CellNavigation", typeof(bool), typeof(TableView), new PropertyMetadata(true));

        public bool CellNavigation
        {
            get { return (bool)GetValue(CellNavigationProperty); }
            set { SetValue(CellNavigationProperty, value); }
        }
        #endregion

        #region ShowVerticalGridLines dependency property
        public static readonly DependencyProperty ShowVerticalGridLinesProperty = DependencyProperty.Register(
            "ShowVerticalGridLines", typeof(bool), typeof(TableView), new PropertyMetadata(true));

        public bool ShowVerticalGridLines
        {
            get { return (bool)GetValue(ShowVerticalGridLinesProperty); }
            set { SetValue(ShowVerticalGridLinesProperty, value); }
        }
        #endregion

        #region ShowHorizontalGridLines dependency property
        public static readonly DependencyProperty ShowHorizontalGridLinesProperty = DependencyProperty.Register(
            "ShowHorizontalGridLines", typeof(bool), typeof(TableView), new PropertyMetadata(false));

        public bool ShowHorizontalGridLines
        {
            get { return (bool)GetValue(ShowHorizontalGridLinesProperty); }
            set { SetValue(ShowHorizontalGridLinesProperty, value); }
        }
        #endregion

        #region ItemsSource dependency property
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(TableView), new FrameworkPropertyMetadata(null, OnItemsSourcePropertyChanged));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tv = d as TableView;
            if (tv != null)
            {
                if (tv.RowsPresenter != null)
                {
                    tv.RowsPresenter.ItemsSource = e.NewValue as IEnumerable;
                }
            }
        }
        #endregion

        #region SelectedRowIndex dependency property
        public static readonly DependencyProperty SelectedRowIndexProperty = DependencyProperty.Register(
            "SelectedRowIndex",
            typeof(object),
            typeof(TableView),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SelectedRowIndex
        {
            get { return (int)GetValue(SelectedRowIndexProperty); }
            set { SetValue(SelectedRowIndexProperty, value); }
        }
        #endregion

        #region SelectedColumnIndex dependency property
        public static readonly DependencyProperty SelectedColumnIndexProperty = DependencyProperty.Register(
            "SelectedColumnIndex",
            typeof(int),
            typeof(TableView),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SelectedColumnIndex
        {
            get { return (int)GetValue(SelectedColumnIndexProperty); }
            set { SetValue(SelectedColumnIndexProperty, value); }
        }
        #endregion

        #region FocusedRowIndex dependency property
        public static readonly DependencyProperty FocusedRowIndexProperty = DependencyProperty.Register(
            "FocusedRowIndex",
            typeof(object),
            typeof(TableView),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int FocusedRowIndex
        {
            get { return (int)GetValue(FocusedRowIndexProperty); }
            set { SetValue(FocusedRowIndexProperty, value); }
        }
        #endregion

        #region FocusedColumnIndex dependency property
        public static readonly DependencyProperty FocusedColumnIndexProperty = DependencyProperty.Register(
            "FocusedColumnIndex",
            typeof(int),
            typeof(TableView),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int FocusedColumnIndex
        {
            get { return (int)GetValue(FocusedColumnIndexProperty); }
            set { SetValue(FocusedColumnIndexProperty, value); }
        }
        #endregion

        #region FixedColumnCount dependency property
        public static readonly DependencyProperty FixedColumnCountProperty = DependencyProperty.Register(
            "FixedColumnCount", typeof(int), typeof(TableView), new FrameworkPropertyMetadata(0, OnFixedColumnsCountPropertyChanged));

        public int FixedColumnCount
        {
            get { return (int)GetValue(FixedColumnCountProperty); }
            set { SetValue(FixedColumnCountProperty, value); }
        }

        private static void OnFixedColumnsCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TableView)d).NotifyPropertyChanged(d, e);
        }
        #endregion

        #region ColumnsSource dependency property
        public static readonly DependencyProperty ColumnsSourceProperty = DependencyProperty.Register(
            "ColumnsSource",
            typeof(ObservableCollection<TableViewColumn>),
            typeof(TableView),
            new PropertyMetadata(null, OnColumnsSourcePropertyChanged));

        public IEnumerable ColumnsSource
        {
            get { return (IEnumerable)GetValue(ColumnsSourceProperty); }
            set { SetValue(ColumnsSourceProperty, value); }
        }

        private static void OnColumnsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tableView = d as TableView;
            if (tableView == null)
            {
                return;
            }

            ObservableCollection<TableViewColumn> columns = null;
            if (e.NewValue != null)
            {
                columns = e.NewValue as ObservableCollection<TableViewColumn> ??
                    new ObservableCollection<TableViewColumn>((IEnumerable<TableViewColumn>)e.NewValue);
            }
            if (columns == null)
            {
                tableView.Columns.Clear();
            }
            else
            {
                tableView.Columns = columns;
            }
        }
        #endregion

        #region HorizontalScrollOffset dependency property
        public static readonly DependencyProperty HorizontalScrollOffsetProperty = DependencyProperty.Register(
            "HorizontalScrollOffset", typeof(double), typeof(TableView), new FrameworkPropertyMetadata(0.0, OnHorizontalOffsetPropertyChanged));

        public double HorizontalScrollOffset
        {
            get { return (double)GetValue(HorizontalScrollOffsetProperty); }
            set { SetValue(HorizontalScrollOffsetProperty, value); }
        }

        private static void OnHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TableView)d).NotifyPropertyChanged(d, e);
        }
        #endregion

        #region CellsPanelStyle dependency property
        public static readonly DependencyProperty CellsPanelStyleProperty = DependencyProperty.Register(
            "CellsPanelStyle", typeof(Style), typeof(TableView), new FrameworkPropertyMetadata(null, OnCellsPanelStyleChanged));

        public Style CellsPanelStyle
        {
            get { return (Style)GetValue(CellsPanelStyleProperty); }
            set { SetValue(CellsPanelStyleProperty, value); }
        }

        private static void OnCellsPanelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TableView)d).NotifyPropertyChanged(d, e);
        }
        #endregion

        #region RowsPanelStyle dependency property
        public static readonly DependencyProperty RowsPanelStyleProperty = DependencyProperty.Register(
            "RowsPanelStyle", typeof(Style), typeof(TableView), new FrameworkPropertyMetadata(null, OnRowsPanelStyleChanged));

        public Style RowsPanelStyle
        {
            get { return (Style)GetValue(RowsPanelStyleProperty); }
            set { SetValue(RowsPanelStyleProperty, value); }
        }

        private static void OnRowsPanelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TableView)d).NotifyPropertyChanged(d, e);
        }
        #endregion

        #region HeaderPanelStyle dependency property
        public static readonly DependencyProperty HeaderPanelStyleProperty = DependencyProperty.Register(
            "HeaderPanelStyle", typeof(Style), typeof(TableView), new FrameworkPropertyMetadata(null, OnHeaderPanelStyleChanged));

        public Style HeaderPanelStyle
        {
            get { return (Style)GetValue(HeaderPanelStyleProperty); }
            set { SetValue(HeaderPanelStyleProperty, value); }
        }

        private static void OnHeaderPanelStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TableView)d).NotifyPropertyChanged(d, e);
        }
        #endregion

        #region GridLinesBrush dependency property
        public static readonly DependencyProperty GridLinesBrushProperty = DependencyProperty.Register(
            "GridLinesBrush", typeof(Brush), typeof(TableView), new FrameworkPropertyMetadata(Brushes.DarkSlateGray));

        public Brush GridLinesBrush
        {
            get { return (Brush)GetValue(GridLinesBrushProperty); }
            set { SetValue(GridLinesBrushProperty, value); }
        }
        #endregion

        #endregion
    }
}