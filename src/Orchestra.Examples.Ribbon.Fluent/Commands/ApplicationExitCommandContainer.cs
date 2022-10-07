namespace Orchestra.Examples.Ribbon
{
    using System;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;

    public class ApplicationExitCommandContainer : Catel.MVVM.CommandContainerBase
    {
        private readonly INavigationService _navigationService;

        public ApplicationExitCommandContainer(ICommandManager commandManager, INavigationService navigationService)
            : base(Commands.Application.Exit, commandManager)
        {
            ArgumentNullException.ThrowIfNull(navigationService);

            _navigationService = navigationService;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _navigationService.CloseApplicationAsync();
        }
    }
}
