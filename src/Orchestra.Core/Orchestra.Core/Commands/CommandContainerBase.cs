// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandContainerBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Catel;
    using Catel.MVVM;

    public abstract class CommandContainerBase : CommandContainerBase<object>
    {
        protected CommandContainerBase(string commandName, ICommandManager commandManager)
            : base(commandName, commandManager)
        {
        }
    }

    public abstract class CommandContainerBase<TParameter> : CommandContainerBase<TParameter, TParameter>
    {
        protected CommandContainerBase(string commandName, ICommandManager commandManager)
            : base(commandName, commandManager)
        {
        }
    }

    public abstract class CommandContainerBase<TExecuteParameter, TCanExecuteParameter>
    {
        #region Fields
        private readonly ICommandManager _commandManager;

        private readonly ICompositeCommand _compositeCommand;
        private readonly ICommand _command;
        #endregion

        #region Constructors
        protected CommandContainerBase(string commandName, ICommandManager commandManager)
        {
            Argument.IsNotNullOrWhitespace(() => commandName);
            Argument.IsNotNull(() => commandManager);

            CommandName = commandName;
            _commandManager = commandManager;

            _compositeCommand = (ICompositeCommand)_commandManager.GetCommand(commandName);
            _command = new Command<TExecuteParameter, TCanExecuteParameter>(Execute, CanExecute);

            _commandManager.RegisterCommand(commandName, _command);
        }
        #endregion

        #region Properties
        public string CommandName { get; private set; }
        #endregion

        #region Methods
        protected void InvalidateCommand()
        {
            _compositeCommand.RaiseCanExecuteChanged();
        }

        protected virtual bool CanExecute(TCanExecuteParameter parameter)
        {
            return true;
        }

        protected virtual async void Execute(TExecuteParameter parameter)
        {
#pragma warning disable 4014
             ExecuteAsync(parameter);
#pragma warning restore 4014
        }

        protected virtual async Task ExecuteAsync(TExecuteParameter parameter)
        {
        }
        #endregion
    }
}