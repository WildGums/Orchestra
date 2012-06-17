// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraBootstrapper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System.Windows;
    using Catel;
    using Catel.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Services;
    using Views;

    /// <summary>
    /// The bootstrapper that will create and run the shell.
    /// </summary>
    public class OrchestraBootstrapper : BootstrapperBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraBootstrapper"/> class.
        /// </summary>
        public OrchestraBootstrapper()
        {
            LogManager.RegisterDebugListener();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the <see cref="T:Microsoft.Practices.Prism.Modularity.IModuleCatalog"/> used by Prism.
        /// </summary>
        /// <returns></returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            var moduleCatalog = new DirectoryModuleCatalog {ModulePath = @".\Modules"};

            moduleCatalog.Initialize();

            return moduleCatalog;
        }

        /// <summary>
        /// Configures the IoC container.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IOrchestraService, OrchestraService>();
        }

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>
        /// The shell of the application.
        /// </returns>
        /// <remarks>
        /// If the returned instance is a <see cref="T:System.Windows.DependencyObject"/>, the
        ///             <see cref="T:Microsoft.Practices.Prism.Bootstrapper"/> will attach the default <seealso cref="T:Microsoft.Practices.Prism.Regions.IRegionManager"/> of
        ///             the application in its <see cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionManagerProperty"/> attached property
        ///             in order to be able to add regions by using the <seealso cref="F:Microsoft.Practices.Prism.Regions.RegionManager.RegionNameProperty"/>
        ///             attached property from XAML.
        /// </remarks>
        protected override DependencyObject CreateShell()
        {
            var shell = new MainWindow();

            shell.Show();

            return shell;
        }
        #endregion
    }
}