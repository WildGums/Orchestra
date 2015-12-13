// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandInfo.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Models
{
    using Catel.Windows.Input;

    public class CommandInfo : ICommandInfo
    {
        #region Constructors
        public CommandInfo(string commandName, InputGesture inputGesture)
        {
            CommandName = commandName;
            InputGesture = inputGesture;
        }
        #endregion

        public string CommandName { get; private set; }

        public InputGesture InputGesture { get; set; } 

        public bool IsHidden { get; set; }
    }
}