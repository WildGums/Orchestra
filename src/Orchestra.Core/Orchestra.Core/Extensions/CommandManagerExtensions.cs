// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandManagerExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Catel;
    using Catel.MVVM;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public static class CommandManagerExtensions
    {
        public static Dictionary<string, ICommand> FindCommandsByGesture(this ICommandManager commandManager, InputGesture inputGesture)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => inputGesture);

            var commands = new Dictionary<string, ICommand>();

            foreach (var commandName in commandManager.GetCommands())
            {
                var commandInputGesture = commandManager.GetInputGesture(commandName);
                if (inputGesture.Equals(commandInputGesture))
                {
                    commands[commandName] = commandManager.GetCommand(commandName);
                }
            }

            return commands;
        }
    }
}