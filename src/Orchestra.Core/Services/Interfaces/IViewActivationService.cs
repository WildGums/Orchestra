namespace Orchestra.Services
{
    using System;
    using Catel.MVVM;

    public interface IViewActivationService
    {
        bool Activate(IViewModel viewModel);
        bool Activate(Type viewModelType);
    }
}
