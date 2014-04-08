// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Modules.ModuleManager;
    using Catel.Modules.ModuleManager.Models;
    using Catel.Reflection;
    using Microsoft.Practices.Prism.Modularity;

    /// <summary>
    /// The about view model.
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        private readonly IModuleInfoManager _moduleInfoManager;
        private readonly IProcessService _processService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel" /> class.
        /// </summary>
        public AboutViewModel(IModuleInfoManager moduleInfoManager, IProcessService processService)
        {
            Argument.IsNotNull(() => moduleInfoManager);
            Argument.IsNotNull(() => processService);

            _moduleInfoManager = moduleInfoManager;
            _processService = processService;

            var knownModules = moduleInfoManager.KnownModules;
            var tempModules = knownModules.Select(moduleInfo => new Models.ModuleInfo(moduleInfo)).ToList();
            //{
            //    ModuleName = moduleInfo.ModuleName,
            //    Enabled = moduleInfo.InitializationMode == InitializationMode.WhenAvailable,
            //    State = moduleInfo.InitializationMode == InitializationMode.WhenAvailable ? "Active" : "OnDemand"                
            //}).ToList();

            // Code creates a new instance of the Module -> this calls InitializeRibbon -> new instances of all modules are loaded.
            //LicenseUrl = ((Modules.ModuleBase)ServiceLocator.ResolveType(TypeCache.GetType(moduleInfo.ModuleType))).GetLicenseUrl()

            var sorted = tempModules.OrderBy(module => module.ModuleName);
            Modules = new ObservableCollection<ModuleTemplate>(sorted);

            CloseCommand = new Command(OnCloseExecute);
            ViewLicense = new Command<Models.ModuleInfo>(OnViewLicenseExecute, OnViewLicenseCanExecute);
        }

        /// <summary>
        /// Gets the modules of the application.
        /// </summary>
        /// <value>The modules.</value>
        public ObservableCollection<ModuleTemplate> Modules { get; private set; }

        /// <summary>
        /// Gets the Close command.
        /// </summary>
        public Command CloseCommand { get; private set; }

        /// <summary>
        /// Method to invoke when the Close command is executed.
        /// </summary>
        private void OnCloseExecute()
        {
            CloseViewModel(true);
        }

        /// <summary>
        /// Gets the ViewLicense command.
        /// </summary>
        public Command<Models.ModuleInfo> ViewLicense { get; private set; }

        /// <summary>
        /// Method to check whether the ViewLicense command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnViewLicenseCanExecute(Models.ModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(moduleInfo.LicenseUrl);
        }

        /// <summary>
        /// Method to invoke when the ViewLicense command is executed.
        /// </summary>
        private void OnViewLicenseExecute(Models.ModuleInfo moduleInfo)
        {
            _processService.StartProcess(moduleInfo.LicenseUrl);
        }
    }
}