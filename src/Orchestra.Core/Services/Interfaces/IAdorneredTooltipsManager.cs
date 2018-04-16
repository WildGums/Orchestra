// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdorneredTooltipsManager.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows;

    public interface IAdorneredTooltipsManager
    {
        #region Properties
        bool IsEnabled { get; }
        #endregion

        #region Methods
        void AddHintsFor(FrameworkElement element);

        void HideHints();
        void ShowHints();

        void Enable();
        void Disable();
        #endregion
    }
}