// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    /// <summary>
    /// The Configuration class contains the configurable items for the Orchestra Shell.
    /// </summary>
    public class Configuration
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// </summary>
        public Configuration()
        {
            HelpTabText = "Help";
            HelpGroupText = "Help";
            HelpButtonText = "Help";
        }
        #endregion

        #region Properties
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
        #endregion
    }
}