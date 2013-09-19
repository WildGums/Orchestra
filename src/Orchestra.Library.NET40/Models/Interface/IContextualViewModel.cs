namespace Orchestra.Models
{
    /// <summary>
    /// Interface can be used to inform a viewmodel that it's View has become active in Orchestra.
    /// </summary>
    public interface IContextualViewModel
    {
        /// <summary>
        /// Method is called when the active view changes within the orchestra application
        /// </summary>
        void ViewModelActivated();
    }
}
