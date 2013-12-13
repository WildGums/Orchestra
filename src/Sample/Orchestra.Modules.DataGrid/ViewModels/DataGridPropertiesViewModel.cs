namespace Orchestra.Modules.DataGrid.ViewModels
{
    /// <summary>
    /// Backing ViewModel for the DataGridPropertiesView
    /// </summary>
    public class DataGridPropertiesViewModel : Orchestra.ViewModels.ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridPropertiesViewModel"/> class.
        /// </summary>
        public DataGridPropertiesViewModel()
        {
            Title = "Datagrid Properties";
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="DataGridPropertiesViewModel"/> class.
        ///// </summary>
        ///// <param name="title">The title.</param>
        //public DataGridPropertiesViewModel(string title) 
        //    : this()
        //{            
        //    Title = title;            
        //}

        /// <summary>
        /// Gets or sets the number of rows in the datagrid.        
        /// </summary>
        /// <value>
        /// The number of rows in the datagrid.
        /// </value>
        public int NumberOfRows { get; set; }
    }
}
