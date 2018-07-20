// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationAboutCommandContainer.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Orchestra;
    using Orchestra.Services;

    public class ApplicationAboutCommandContainer : Catel.MVVM.CommandContainerBase
    {
        #region Fields
        private readonly IAboutService _aboutService;
        #endregion

        #region Constructors
        public ApplicationAboutCommandContainer(ICommandManager commandManager, IAboutService aboutService)
            : base(Commands.Application.About, commandManager)
        {
            Argument.IsNotNull(() => aboutService);

            _aboutService = aboutService;
        }
        #endregion

        #region Methods
        protected override void Execute(object parameter)
        {
            _aboutService.ShowAboutAsync();
        }
        #endregion
    }
}