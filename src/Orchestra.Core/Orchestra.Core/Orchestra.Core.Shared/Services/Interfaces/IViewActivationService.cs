// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewActivationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


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