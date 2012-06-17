// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleBase.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules
{
    using System;
    using Microsoft.Practices.Prism.Modularity;

    /// <summary>
    /// Base class for all modules used by Orchestra.
    /// </summary>
    [Module]
    public abstract class ModuleBase : IModule, IServiceProvider
    {
        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Notifies the module that it has be initialized.
        /// </summary>
        public void Initialize()
        {
            OnInitializing();

            OnInitialized();
        }

        /// <summary>
        /// Called when the module is initializing.
        /// </summary>
        protected virtual void OnInitializing()
        {
        }

        /// <summary>
        /// Called when the module has been initialized.
        /// </summary>
        protected virtual void OnInitialized()
        {
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <param name="serviceType">An object that specifies the type of service object to get.</param>
        /// <returns>A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.</returns>
        public object GetService(Type serviceType)
        {
            return Catel.IoC.ServiceLocator.Instance.ResolveType(serviceType);
        }

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the service.</typeparam>
        /// <returns>The service instance.</returns>
        public T GetService<T>()
        {
            return (T) GetService(typeof (T));
        }
        #endregion
    }
}