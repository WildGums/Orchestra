// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraBootstrapper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2012 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Logging;
    using Catel.Reflection;
    using Microsoft.Practices.Prism.Modularity;
    using Modules;
    using Services;
    using Views;

    /// <summary>
    /// The bootstrapper that will create and run the shell.
    /// </summary>
    public class OrchestraBootstrapper : BootstrapperBase<MainWindow>
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraBootstrapper"/> class.
        /// </summary>
        public OrchestraBootstrapper()
        {
#if DEBUG
            LogManager.RegisterDebugListener();
#endif

            Log.Debug("Optimizing performance by disable the WarningAndErrorValidator in Catel");

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            var appDomain = AppDomain.CurrentDomain;
            appDomain.AssemblyResolve += OnAssemblyResolve;

            string modulesDirectory = ModulesDirectory;
            if (!Directory.Exists(modulesDirectory))
            {
                Log.Warning("Modules path '{0}' is missing, creating it", modulesDirectory);

                Directory.CreateDirectory(modulesDirectory);
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// The modules directory.
        /// </summary>
        private string ModulesDirectory { get { return Path.Combine(".", ModuleBase.ModulesDirectory); } }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the <see cref="T:Microsoft.Practices.Prism.Modularity.IModuleCatalog"/> used by Prism.
        /// </summary>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            var moduleCatalog = new DirectoryModuleCatalog { ModulePath = ModulesDirectory };

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
            Container.RegisterType<IRibbonService, RibbonService>();
        }

        /// <summary>
        /// Called when the resolving of assemblies failed. In that case, this method will try to load the 
        /// assemblies from the modules directory.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns>The assembly or <c>null</c> if the assembly could not be resolved.</returns>
        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = TypeHelper.GetAssemblyNameWithoutOverhead(args.Name);

            var files = Directory.GetFiles(ModuleBase.ModulesDirectory, "*.dll", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var fileAssemblyName = fileInfo.Name;
                int extensionStart = fileAssemblyName.LastIndexOf(fileInfo.Extension);
                if (extensionStart > 0)
                {
                    fileAssemblyName = fileAssemblyName.Substring(0, extensionStart);
                }

                if (string.Equals(fileAssemblyName, assemblyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return ResolveAssemblyAndReferencedAssemblies(file);
                }
            }

            Log.Error("Failed to delay resolve assembly '{0}'", args.Name);
            return null;
        }

        /// <summary>
        /// Resolves the assembly and referenced assemblies.
        /// </summary>
        private Assembly ResolveAssemblyAndReferencedAssemblies(string assemblyFileName)
        {
            Log.Debug("Resolving assembly '{0}' manually", assemblyFileName);

            var appDomain = AppDomain.CurrentDomain;
            var assemblyDirectory = Catel.IO.Path.GetParentDirectory(assemblyFileName);

            // Load references
            var assemblyForReflectionOnly = Assembly.ReflectionOnlyLoadFrom(assemblyFileName);
            foreach (var referencedAssembly in assemblyForReflectionOnly.GetReferencedAssemblies())
            {
                if (!appDomain.GetAssemblies().Any(a => string.CompareOrdinal(a.GetName().Name, referencedAssembly.Name) == 0))
                {
                    // First, try to load from GAC
                    if (referencedAssembly.GetPublicKeyToken() != null)
                    {
                        try
                        {
                            appDomain.Load(referencedAssembly.FullName);
                            continue;
                        }
                        catch (Exception)
                        {
                            Log.Debug("Failed to load assembly '{0}' from GAC, trying local file", referencedAssembly.FullName);
                        }
                    }

                    // Second, try to load from directory
                    var referencedAssemblyPath = Path.Combine(assemblyDirectory, referencedAssembly.Name + ".dll");
                    ResolveAssemblyAndReferencedAssemblies(referencedAssemblyPath);
                }
            }

            // Load assembly itself
            var assembly = Assembly.LoadFrom(assemblyFileName);

            Log.Info("Resolved assembly '{0}' manually", assemblyFileName);

            return assembly;
        }
        #endregion
    }
}