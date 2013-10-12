namespace Orchestra.Modules.DataGrid.ViewModels
{
    /// <summary>
    /// Backing ViewModel for the DataGridPropertiesView
    /// </summary>
    public class DataGridPropertiesViewModel : Orchestra.ViewModels.ViewModelBase
    {
        public DataGridPropertiesViewModel() 
            : base()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridPropertiesViewModel"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public DataGridPropertiesViewModel(string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                Title = title;
            }
        }
    }
}
