// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel.MVVM;
    using Catel.Modules.ModuleManager;
    using Catel.Modules.ModuleManager.Models;
    using Microsoft.Practices.Prism.Modularity;

    /// <summary>
    /// The about view model.
    /// </summary>
    public class AboutViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutViewModel" /> class.
        /// </summary>
        public AboutViewModel()
        {
            var knownModules = GetService<IModuleInfoManager>().KnownModules;
            var tempModules = knownModules.Select(moduleInfo => new ModuleTemplate
            {
                ModuleName = moduleInfo.ModuleName,
                Enabled = moduleInfo.InitializationMode == InitializationMode.WhenAvailable,
                State = moduleInfo.InitializationMode == InitializationMode.WhenAvailable ? "Active" : "OnDemand"
            }).ToList();

            var sorted = tempModules.OrderBy(module => module.ModuleName);
            Modules = new ObservableCollection<ModuleTemplate>(sorted);

            CloseCommand = new Command(OnCloseExecute);
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
    }
}