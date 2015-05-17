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

    [ObsoleteEx(ReplacementTypeOrMember = "Catel.MVVM.CommandContainerBase", TreatAsErrorFromVersion = "2.2.0", RemoveInVersion = "3.0.0")]
    public abstract class CommandContainerBase : CommandContainerBase<object>
    {
        #region Constructors
        protected CommandContainerBase(string commandName, ICommandManager commandManager)
            : base(commandName, commandManager)
        {
        }
        #endregion
    }

    [ObsoleteEx(ReplacementTypeOrMember = "Catel.MVVM.CommandContainerBase", TreatAsErrorFromVersion = "2.2.0", RemoveInVersion = "3.0.0")]
    public abstract class CommandContainerBase<TParameter> : CommandContainerBase<TParameter, TParameter, ITaskProgressReport>
    {
        #region Constructors
        protected CommandContainerBase(string commandName, ICommandManager commandManager)
            : base(commandName, commandManager)
        {
        }
        #endregion
    }

    [ObsoleteEx(ReplacementTypeOrMember = "Catel.MVVM.CommandContainerBase", TreatAsErrorFromVersion = "2.2.0", RemoveInVersion = "3.0.0")]
    public abstract class CommandContainerBase<TExecuteParameter, TCanExecuteParameter> : CommandContainerBase<TExecuteParameter, TCanExecuteParameter, ITaskProgressReport>
    {
        #region Constructors
        protected CommandContainerBase(string commandName, ICommandManager commandManager)
            : base(commandName, commandManager)
        {
        }
        #endregion
    }

    [ObsoleteEx(ReplacementTypeOrMember = "Catel.MVVM.CommandContainerBase", TreatAsErrorFromVersion = "2.2.0", RemoveInVersion = "3.0.0")]
    public abstract class CommandContainerBase<TExecuteParameter, TCanExecuteParameter, TPogress> 
        where TPogress : ITaskProgressReport
    {
        #region Fields
        private readonly ICommand _command;
        private readonly ICommandManager _commandManager;
        private readonly ICompositeCommand _compositeCommand;
        #endregion

        #region Constructors
        protected CommandContainerBase(string commandName, ICommandManager commandManager)
        {
            Argument.IsNotNullOrWhitespace(() => commandName);
            Argument.IsNotNull(() => commandManager);

            CommandName = commandName;
            _commandManager = commandManager;

            _compositeCommand = (ICompositeCommand) _commandManager.GetCommand(commandName);
            _command = new TaskCommand<TExecuteParameter, TCanExecuteParameter, TPogress>(Execute, CanExecute);

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

        [ObsoleteEx(ReplacementTypeOrMember = "Execute", TreatAsErrorFromVersion = "3.0.0", RemoveInVersion = "4.0.0")]
        protected virtual async Task ExecuteAsync(TExecuteParameter parameter)
        {
            
        }

        protected virtual async Task Execute(TExecuteParameter parameter)
        {
            await ExecuteAsync(parameter);
        }
        
        #endregion
    }
}