// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleBase.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules
{
    using System;
    using Microsoft.Practices.Prism.Modularity;
    using Services;

    /// <summary>
    /// Base class for all modules used by Orchestra.
    /// </summary>
    [Module]
    public abstract class ModuleBase : Catel.Modules.ModuleBase
    {
        /// <summary>
        /// The modules directory name.
        /// </summary>
        public const string ModulesDirectory = "Modules";

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleBase"/> class.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <exception cref="ArgumentException">The <paramref name="moduleName"/> is <c>null</c> or whitespace.</exception>
        protected ModuleBase(string moduleName) 
            : base(moduleName)
        {
        }

        /// <summary>
        /// Initializes the ribbon.
        /// <para />
        /// Use this method to hook up views to ribbon items.
        /// </summary>
        /// <param name="ribbonService">The ribbon service.</param>
        public virtual void InitializeRibbon(IRibbonService ribbonService)
        {
            
        }
    }
}