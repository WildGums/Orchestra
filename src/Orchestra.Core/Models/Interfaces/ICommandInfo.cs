// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICommandInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using Catel.Windows.Input;

    public interface ICommandInfo
    {
        #region Properties
        string CommandName { get; }
        InputGesture InputGesture { get; set; }
        bool IsHidden { get; set; }
        #endregion
    }
}