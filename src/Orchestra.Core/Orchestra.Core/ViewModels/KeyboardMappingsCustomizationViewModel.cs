// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappingsViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Text;
    using Catel.Windows.Input;
    using Orchestra.Services;

    public class KeyboardMappingsCustomizationViewModel : ViewModelBase
    {
        private readonly IKeyboardMappingsService _keyboardMappingsService;
        private readonly ICommandManager _commandManager;
        private readonly ILanguageService _languageService;
        private readonly IMessageService _messageService;

        public KeyboardMappingsCustomizationViewModel(IKeyboardMappingsService keyboardMappingsService, ICommandManager commandManager,
            ILanguageService languageService, IMessageService messageService)
        {
            Argument.IsNotNull(() => keyboardMappingsService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => languageService);
            Argument.IsNotNull(() => messageService);

            _keyboardMappingsService = keyboardMappingsService;
            _commandManager = commandManager;
            _languageService = languageService;
            _messageService = messageService;

            Commands = new ObservableCollection<string>();
            CommandFilter = string.Empty;
            SelectedCommand = string.Empty;

            Reset = new Command(OnResetExecute);
            Remove = new Command(OnRemoveExecute, OnRemoveCanExecute);
            Assign = new Command(OnAssignExecute, OnAssignCanExecute);
        }

        #region Properties
        public override string Title
        {
            get { return _languageService.GetString("KeyboardShortcuts"); }
        }

        public string CommandFilter { get; set; }

        public ObservableCollection<string> Commands { get; private set; }

        public string SelectedCommand { get; set; }

        public InputGesture SelectedCommandInputGesture { get; private set; }

        public InputGesture SelectedCommandNewInputGesture { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the Reset command.
        /// </summary>
        public Command Reset { get; private set; }

        /// <summary>
        /// Method to invoke when the Reset command is executed.
        /// </summary>
        private void OnResetExecute()
        {
            _keyboardMappingsService.Reset();

            if (!string.IsNullOrWhiteSpace(SelectedCommand))
            {
                SelectedCommandInputGesture = _commandManager.GetInputGesture(SelectedCommand);
            }
        }

        /// <summary>
        /// Gets the Remove command.
        /// </summary>
        public Command Remove { get; private set; }

        /// <summary>
        /// Method to check whether the Remove command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnRemoveCanExecute()
        {
            if (string.IsNullOrWhiteSpace(SelectedCommand))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to invoke when the Remove command is executed.
        /// </summary>
        private void OnRemoveExecute()
        {
            SelectedCommandInputGesture = null;
            _commandManager.UpdateInputGesture(SelectedCommand, null);
        }

        /// <summary>
        /// Gets the Assign command.
        /// </summary>
        public Command Assign { get; private set; }

        /// <summary>
        /// Method to check whether the Assign command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnAssignCanExecute()
        {
            if (string.IsNullOrWhiteSpace(SelectedCommand))
            {
                return false;
            }

            if (SelectedCommandNewInputGesture == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to invoke when the Assign command is executed.
        /// </summary>
        private async void OnAssignExecute()
        {
            SelectedCommandInputGesture = SelectedCommandNewInputGesture;

            var selectedCommand = SelectedCommand;
            var selectedInputGesture = SelectedCommandInputGesture;

            if (!selectedInputGesture.IsEmpty())
            {
                var existingCommands = _commandManager.FindCommandsByGesture(selectedInputGesture);
                if (existingCommands.Any())
                {
                    var messageBuilder = new StringBuilder();

                    messageBuilder.AppendLine("The input gesture '{0}' is currently being used by the following commands:", selectedInputGesture);
                    messageBuilder.AppendLine();

                    foreach (var existingCommand in existingCommands)
                    {
                        messageBuilder.AppendLine("- {0}", existingCommand.Key);
                    }

                    messageBuilder.AppendLine();
                    messageBuilder.AppendLine("Are you sure you want to assign the input gesture to '{0}'. It will be removed from the other commands.",
                        selectedCommand);

                    if (await _messageService.Show(messageBuilder.ToString(), "Replace input gesture?", MessageButton.YesNo) == MessageResult.No)
                    {
                        return;
                    }

                    foreach (var existingCommand in existingCommands)
                    {
                        _commandManager.UpdateInputGesture(existingCommand.Key, null);
                    }
                }
            }

            _commandManager.UpdateInputGesture(selectedCommand, selectedInputGesture);
        }
        #endregion

        #region Methods
        protected override async Task Close()
        {
            _keyboardMappingsService.Save();
        }

        private void OnCommandFilterChanged()
        {
            var allCommands = _commandManager.GetCommands().OrderBy(x => x).ToList();
            if (!string.IsNullOrWhiteSpace(CommandFilter))
            {
                allCommands = allCommands.Where(x =>  x.IndexOf(CommandFilter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            if (Commands == null)
            {
                Commands = new ObservableCollection<string>(allCommands);
            }
            else
            {
                Commands.ReplaceRange(allCommands);
            }
        }

        private void OnSelectedCommandChanged()
        {
            InputGesture inputGesture = null;

            if (!string.IsNullOrWhiteSpace(SelectedCommand))
            {
                inputGesture = _commandManager.GetInputGesture(SelectedCommand);
            }

            SelectedCommandInputGesture = inputGesture;
            SelectedCommandNewInputGesture = null;
        }
        #endregion
    }
}