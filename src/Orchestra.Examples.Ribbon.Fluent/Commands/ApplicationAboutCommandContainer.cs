namespace Orchestra.Examples.Ribbon
{
    using System;
    using Catel.MVVM;
    using Orchestra.Services;

    public class ApplicationAboutCommandContainer : Catel.MVVM.CommandContainerBase
    {
        private readonly IAboutService _aboutService;

        public ApplicationAboutCommandContainer(ICommandManager commandManager, IAboutService aboutService)
            : base(Commands.Application.About, commandManager)
        {
            ArgumentNullException.ThrowIfNull(aboutService);

            _aboutService = aboutService;
        }

        public override void Execute(object parameter)
        {
            _aboutService.ShowAboutAsync();
        }
    }
}
