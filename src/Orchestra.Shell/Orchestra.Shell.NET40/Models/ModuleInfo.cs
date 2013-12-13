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
        /// Initializes a new instance of the <see cref="ModuleInfo"/> class.
        /// </summary>
        /// <param name="moduleInfo">The module information.</param>
        public ModuleInfo(Microsoft.Practices.Prism.Modularity.ModuleInfo moduleInfo)
            : base(moduleInfo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleInfo"/> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="moduleType">Type of the module.</param>
        public ModuleInfo(string moduleName, string moduleType)
            : base(new Microsoft.Practices.Prism.Modularity.ModuleInfo(moduleName, moduleType))
        {
        }

        /// <summary>
        /// Gets or sets the license URL.
        /// </summary>
        /// <value>The license URL.</value>
        public string LicenseUrl { get; set; }
    }
}