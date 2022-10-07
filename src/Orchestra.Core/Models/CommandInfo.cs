namespace Orchestra
{
    using System;
    using Catel.Windows.Input;

    public class CommandInfo : ICommandInfo
    {
        public CommandInfo(string commandName, InputGesture? inputGesture)
        {
            ArgumentNullException.ThrowIfNull(commandName);

            CommandName = commandName;
            InputGesture = inputGesture;
        }

        public string CommandName { get; private set; }

        public InputGesture? InputGesture { get; set; } 

        public bool IsHidden { get; set; }
    }
}
