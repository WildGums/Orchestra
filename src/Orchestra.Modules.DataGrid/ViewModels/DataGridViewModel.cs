// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.ViewModels
{
    using System.Data;
    using System.IO;
    using System.Linq;

    using Catel.Data;
    using Catel.MVVM;
    using Catel.MVVM.Services;

    using CsvHelper;

    using ViewModelBase = Orchestra.ViewModels.ViewModelBase;

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
            get { return "Data grid"; }
        }

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
                return;

            var reader = new CsvReader(new StreamReader(openFileService.FileName));
            if (!reader.Read())
                return;

            var dataTable = new DataTable();
            dataTable.Columns.AddRange(reader.FieldHeaders.Select(columnName => new DataColumn(columnName)).ToArray());

            do
            {
                dataTable.Rows.Add(reader.CurrentRecord);
            }
            while (reader.Read());

            DataTable = dataTable;
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
                return;

            using (var writer = new CsvWriter(new StreamWriter(saveFileService.FileName)))
            {
                DataTable dataTable = DataTable;

                // Writing columns.
                foreach (DataColumn column in dataTable.Columns)
                    writer.WriteField(column);
                writer.NextRecord();

                // Writing data rows.
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                        writer.WriteField(row[column]);
                    writer.NextRecord();
                }
            }
        }
        #endregion

        #region DataTable property
        /// <summary>
        /// DataTable property data.
        /// </summary>
        public static readonly PropertyData DataTableProperty = RegisterProperty("DataTable", typeof(DataTable));

        /// <summary>
        /// Gets or sets the DataTable value.
        /// </summary>
        public DataTable DataTable
        {
            get { return GetValue<DataTable>(DataTableProperty); }
            set { SetValue(DataTableProperty, value); }
        }
        #endregion
    }
}