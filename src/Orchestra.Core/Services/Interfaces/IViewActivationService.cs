// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewActivationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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