// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Orchestra.ViewModels
{
    using System;
    using System.Reflection;
    using Catel.MVVM;
    using Catel.Reflection;
    using Services;
    using Catel.IoC;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Constructor & destructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        public MainWindowViewModel()
            : base()
        {            
            var orchestraService = Catel.IoC.ServiceLocator.Default.ResolveType<IOrchestraService>();
            var viewModelFactory = Catel.IoC.ServiceLocator.Default.ResolveType<IViewModelFactory>();

            Catel.IoC.ServiceLocator.Default.RegisterInstance<IContextualViewModelManager>(new ContextualViewModelManager(orchestraService, viewModelFactory));                  
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get
            {
                return Assembly.GetExecutingAssembly().Title();
            }
        }
        #endregion
    }
}