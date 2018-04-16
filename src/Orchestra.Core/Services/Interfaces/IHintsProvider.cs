// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHintsProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Windows;
    using Models;

    public interface IHintsProvider
    {
        #region Methods
        IList<IHint> GetHintsFor(FrameworkElement element);
        #endregion
    }
}