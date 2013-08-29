namespace Orchestra.Models
{
    /// <summary>
    /// The Configuration class contains the configurable items for the Orchestra Shell.
    /// </summary>
    public class Configuration
    {

        /// <summary>
        /// Gets or sets the name of the 'about' tab.
        /// </summary>
        /// <value>
        /// The name of the 'about' tab.
        /// </value>
        public string HelpTabText { get; set; }

        /// <summary>
        /// Gets or sets the text for the 'About' group in the ribbon.
        /// </summary>
        /// <value>
        /// The about group text.
        /// </value>
        public string HelpGroupText { get; set; }

        /// <summary>
        /// Gets or sets the text for the help button in the Ribbon.
        /// </summary>
        /// <value>
        /// The help button text.
        /// </value>
        public string HelpButtonText { get; set; }
    }
}
