// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertiesViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Modules.Browser.ViewModels
{
    /// <summary>
    /// Backing ViewModel for the PropertiesView
    /// </summary>
    public class PropertiesViewModel : Orchestra.ViewModels.ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesViewModel"/> class.
        /// </summary>
        public PropertiesViewModel(string title) 
            : this()
        {
            Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesViewModel"/> class.
        /// </summary>
        public PropertiesViewModel()
        {
            Title = "Properties";
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
        #endregion
    }
}