// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraBootstrapper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using Catel.Reflection;
    using Microsoft.Practices.Prism.Modularity;
    using Models;
    using Modules;
    using Services;
    using ViewModels;
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

        private readonly bool _createAboutRibbon;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="OrchestraBootstrapper" /> class.
        /// </summary>
        /// <param name="createAboutRibbon">if set to <c>true</c>, a ribbon item for the about box will be created.</param>
        public OrchestraBootstrapper(bool createAboutRibbon = true)
        {
#if DEBUG
            LogManager.RegisterDebugListener();
#endif

            _createAboutRibbon = createAboutRibbon;

            Log.Debug("Optimizing performance by disable the WarningAndErrorValidator in Catel");

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            Log.Info("Loading application themes");

            // Force loading of type
            var ribbonType = typeof(Fluent.Ribbon);
            Log.Debug("Loaded ribbon type '{0}'", ribbonType.Name);

            var application  = Application.Current;
            application.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Fluent;Component/Themes/Office2010/Silver.xaml", UriKind.RelativeOrAbsolute)
            });
            application.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("/Catel.Extensions.Controls;component/themes/generic.xaml", UriKind.RelativeOrAbsolute)
            });

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
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog.
        /// </summary>
        protected override void InitializeModules()
        {
            base.InitializeModules();

            if (_createAboutRibbon)
            {
                var ribbonService = Container.ResolveType<IRibbonService>();
                ribbonService.RegisterRibbonItem(new RibbonButton("Orchestra", "Help", "About", new Command(() =>
                {
                    var uiVisualizerService = Container.ResolveType<IUIVisualizerService>();
                    uiVisualizerService.ShowDialog(new AboutViewModel());
                })));
            }
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