// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrchestraBootstrapper.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Documents;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Modules;
    using Catel.MVVM;
    using Catel.MVVM.Services;
    using Catel.Reflection;
    using Catel.Windows.Threading;    
    using Microsoft.Practices.Prism.Modularity;
    using Models;
    using Services;
    using ViewModels;
    using Views;
    using ModuleBase = Orchestra.Modules.ModuleBase;

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
        /// <param name="createAboutRibbon">If set to <c>true</c>, a ribbon item for the about box will be created.</param>
        public OrchestraBootstrapper(bool createAboutRibbon = true)
        {
#if DEBUG
            LogManager.RegisterDebugListener();
#endif

            _createAboutRibbon = createAboutRibbon;

            Log.Debug("Optimizing performance by disabling the WarningAndErrorValidator in Catel");

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            Log.Info("Loading application themes");

            // Force loading of type
            var ribbonType = typeof(Fluent.Ribbon);
            Log.Debug("Loaded ribbon type '{0}'", ribbonType.Name);

            var application = Application.Current;
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

            CreatedShell += (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var statusBarService = ServiceLocator.Default.ResolveType<IStatusBarService>();
                    statusBarService.UpdateStatus("Ready");
                });
            };
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
            var moduleCatalog = new SafeDirectoryModuleCatalog { ModulePath = ModulesDirectory };

            return moduleCatalog;
        }

        /// <summary>
        /// Configures the IoC container.
        /// </summary>
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IOrchestraService, OrchestraService>();
            Container.RegisterType<IStatusBarService, StatusBarService>();
            Container.RegisterType<IRibbonService, RibbonService>();
            Container.RegisterInstance<IConfigurationService>(new ConfigurationService());

            //Container.RegisterType<IContextualViewModelManager>(registrationType:RegistrationType.Singleton);

           
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules Catalog.
        /// </summary>
        protected override void InitializeModules()
        {
            var configurationService = (IConfigurationService)ServiceLocator.Default.GetService(typeof(IConfigurationService));

            base.InitializeModules();

            if (_createAboutRibbon)
            {
                var ribbonService = Container.ResolveType<IRibbonService>();
                ribbonService.RegisterRibbonItem(new RibbonButton(configurationService.Configuration.HelpTabText, configurationService.Configuration.HelpGroupText, configurationService.Configuration.HelpButtonText, new Command(() =>
                {
                    var uiVisualizerService = Container.ResolveType<IUIVisualizerService>();
                    var typeFactory = TypeFactory.Default;
                    var aboutViewModel = typeFactory.CreateInstance<AboutViewModel>();

                    uiVisualizerService.ShowDialog(aboutViewModel);
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

            // Load shell files
            var files = new List<string>(Directory.GetFiles(typeof(OrchestraBootstrapper).Assembly.GetDirectory(), "*.dll"));

            // Load module files
            files.AddRange(Directory.GetFiles(ModuleBase.ModulesDirectory, "*.dll", SearchOption.AllDirectories));
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
                    var dependencyResolver = this.GetDependencyResolver();
                    var missingAssemblyResolverService = dependencyResolver.Resolve<IMissingAssemblyResolverService>();
                    if (missingAssemblyResolverService != null)
                    {
                        return missingAssemblyResolverService.ResolveAssembly(fileInfo.FullName);
                    }

                    return null;
                }
            }

            Log.Error("Failed to delay resolve assembly '{0}'", args.Name);
            return null;
        }


        #endregion
    }
}