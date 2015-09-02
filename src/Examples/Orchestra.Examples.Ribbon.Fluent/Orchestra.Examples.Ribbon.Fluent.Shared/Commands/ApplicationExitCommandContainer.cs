// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationExitCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Orchestra;

    public class ApplicationExitCommandContainer : Catel.MVVM.CommandContainerBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        #endregion

        #region Constructors
        public ApplicationExitCommandContainer(ICommandManager commandManager, INavigationService navigationService)
            : base(Commands.Application.Exit, commandManager)
        {
            Argument.IsNotNull(() => navigationService);

            _navigationService = navigationService;
        }
        #endregion

        #region Methods
        protected override void Execute(object parameter)
        {
            _navigationService.CloseApplication();
        }
        #endregion
    }
}