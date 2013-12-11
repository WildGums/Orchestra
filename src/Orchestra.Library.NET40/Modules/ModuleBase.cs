// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleBase.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules
{
    using System;
    using Catel.IoC;
    using Microsoft.Practices.Prism.Modularity;
    using Services;

    /// <summary>
    /// Base class for all modules used by Orchestra.
    /// </summary>
    [Module]
    public class ModuleBase : Catel.Modules.ModuleBase, INeedCustomInitialization
    {                
        /// <summary>
        /// The modules directory name.
        /// </summary>
        public const string ModulesDirectory = "Modules";

        /// <summary>
        /// Gets the license URL.
        /// <para />
        /// If this method returns an empty string, it is assumed the module has no license.
        /// </summary>
        /// <returns>The url of the license.</returns>
        public virtual string GetLicenseUrl()
        {
            return string.Empty;
        }

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
        protected virtual void InitializeRibbon(IRibbonService ribbonService)
        {
            
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void INeedCustomInitialization.Initialize()
        {
            var dependencyResolver = this.GetDependencyResolver();
            var ribbonService = dependencyResolver.Resolve<IRibbonService>();

            InitializeRibbon(ribbonService);
        }
    }
}