// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows;

    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.MVVM.Services;

    using CsvHelper;

    using Orchestra.Modules.DataGrid.Models;

    using TableView;

    /// <summary>
    /// The data grid view model.
    /// </summary>
    public class DataGridViewModel : ViewModelBase
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        public override string Title
        {
            get { return "DataGrid"; }
        }

        #region Items property
        /// <summary>
        /// Items property data.
        /// </summary>
        public static readonly PropertyData ItemsProperty = RegisterProperty(
            "Items", typeof(ObservableCollection<Row>), () => new ObservableCollection<Row>());

        /// <summary>
        /// Gets or sets the Items value.
        /// </summary>
        public ObservableCollection<Row> Items
        {
            get { return GetValue<ObservableCollection<Row>>(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }
        #endregion

        #region OpenFile command
        private Command _openFileCommand;

        /// <summary>
        /// Gets the OpenFile command.
        /// </summary>
        public Command OpenFileCommand
        {
            get { return _openFileCommand ?? (_openFileCommand = new Command(OpenFile)); }
        }

        /// <summary>
        /// Method to invoke when the OpenFile command is executed.
        /// </summary>
        private void OpenFile()
        {
            var openFileService = GetService<IOpenFileService>();
            openFileService.Filter = "*.csv|*.csv";
            if (!openFileService.DetermineFile())
            {
                return;
            }

            ObservableCollection<Row> items = Items;
            ObservableCollection<TableViewColumn> columns = Columns;

            Debug.Assert(items != null);
            Debug.Assert(columns != null);

            items.Clear();
            columns.Clear();

            var reader = new CsvReader(new StreamReader(openFileService.FileName));
            if (!reader.Read())
            {
                return;
            }

            for (int i = 0; i < reader.FieldHeaders.Length; i++)
            {
                string fieldHeader = reader.FieldHeaders[i];
                Columns.Add(
                    new TableViewColumn { Title = fieldHeader, ContextBindingPath = string.Format("Cells[{0}]", i), Padding = new Thickness(0) });
            }

            do
            {
                if (reader.CurrentRecord == null)
                {
                    continue;
                }

                var row = new Row();
                for (int i = 0; i < Columns.Count; i++)
                {
                    row.Cells.Add(new StringCell(reader.CurrentRecord[i]));
                }
                Items.Add(row);
            }
            while (reader.Read());
        }
        #endregion

        #region SaveToFile command
        private Command _saveToFileCommand;

        /// <summary>
        /// Gets the SaveToFile command.
        /// </summary>
        public Command SaveToFileCommand
        {
            get { return _saveToFileCommand ?? (_saveToFileCommand = new Command(SaveToFile)); }
        }

        /// <summary>
        /// Method to invoke when the SaveToFile command is executed.
        /// </summary>
        private void SaveToFile()
        {
            var saveFileService = GetService<ISaveFileService>();
            saveFileService.Filter = "*.csv|*.csv";
            if (!saveFileService.DetermineFile())
            {
                return;
            }

            using (var writer = new CsvWriter(new StreamWriter(saveFileService.FileName)))
            {
                ObservableCollection<Row> items = Items;

                // Writing columns.
                foreach (TableViewColumn column in Columns)
                {
                    writer.WriteField(column.Title);
                }
                writer.NextRecord();

                // Writing data rows.
                foreach (Row row in items)
                {
                    foreach (int iColumn in Enumerable.Range(0, Columns.Count))
                    {
                        writer.WriteField(row.Cells[iColumn].Value);
                    }
                    writer.NextRecord();
                }
            }
        }
        #endregion

        #region Columns property
        /// <summary>
        /// Columns property data.
        /// </summary>
        public static readonly PropertyData ColumnsProperty = RegisterProperty(
            "Columns", typeof(ObservableCollection<TableViewColumn>), () => new ObservableCollection<TableViewColumn>());

        /// <summary>
        /// Gets or sets the Columns value.
        /// </summary>
        public ObservableCollection<TableViewColumn> Columns
        {
            get { return GetValue<ObservableCollection<TableViewColumn>>(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        #endregion

        private static string GetSafeColumnName(string columnName)
        {
            Argument.IsNotNullOrWhitespace("columnName", columnName);
            return columnName.Replace(" ", "_").Replace("-", "_").Replace("/", "_").Replace(".", "_");
        }
    }
}