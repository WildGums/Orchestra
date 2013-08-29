// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleInfo.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Models
{
    using Catel.Modules.ModuleManager.Models;

    /// <summary>
    /// Module info container.
    /// </summary>
    public class ModuleInfo : ModuleTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfo" /> class.
        /// </summary>
        public ModuleInfo()
        {
            
        }

        /// <summary>
        /// Gets or sets the license URL.
        /// </summary>
        /// <value>The license URL.</value>
        public string LicenseUrl { get; set; }
    }
}