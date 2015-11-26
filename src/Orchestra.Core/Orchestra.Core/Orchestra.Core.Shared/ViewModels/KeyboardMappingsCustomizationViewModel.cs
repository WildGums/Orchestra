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
    using Models;
    using Orchestra.Services;

    public class KeyboardMappingsCustomizationViewModel : ViewModelBase
    {
        private readonly IKeyboardMappingsService _keyboardMappingsService;
        private readonly ICommandManager _commandManager;
        private readonly ICommandInfoService _commandInfoService;
        private readonly ILanguageService _languageService;
        private readonly IMessageService _messageService;

        public KeyboardMappingsCustomizationViewModel(IKeyboardMappingsService keyboardMappingsService, ICommandManager commandManager,
            ICommandInfoService commandInfoService, ILanguageService languageService, IMessageService messageService)
        {
            Argument.IsNotNull(() => keyboardMappingsService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => commandInfoService);
            Argument.IsNotNull(() => languageService);
            Argument.IsNotNull(() => messageService);

            _keyboardMappingsService = keyboardMappingsService;
            _commandManager = commandManager;
            _commandInfoService = commandInfoService;
            _languageService = languageService;
            _messageService = messageService;

            Commands = new FastObservableCollection<ICommandInfo>();
            CommandFilter = string.Empty;
            SelectedCommand = string.Empty;

            Reset = new TaskCommand(OnResetExecuteAsync);
            Remove = new Command(OnRemoveExecute, OnRemoveCanExecute);
            Assign = new TaskCommand(OnAssignExecuteAsync, OnAssignCanExecute);
        }

        #region Properties
        public override string Title
        {
            get { return _languageService.GetString("Orchestra_KeyboardShortcuts"); }
        }

        public string CommandFilter { get; set; }

        public FastObservableCollection<ICommandInfo> Commands { get; private set; }

        public string SelectedCommand { get; set; }

        public InputGesture SelectedCommandInputGesture { get; private set; }

        public InputGesture SelectedCommandNewInputGesture { get; set; }
        #endregion

        #region Commands
        public TaskCommand Reset { get; private set; }

        private async Task OnResetExecuteAsync()
        {
            var messageResult = await _messageService.ShowAsync(_languageService.GetString("Orchestra_ResetKeyboardShortcutsAreYouSure"), string.Empty, MessageButton.YesNo, MessageImage.Question);
            if (messageResult == MessageResult.No)
            {
                return;
            }

            _keyboardMappingsService.Reset();

            if (!string.IsNullOrWhiteSpace(SelectedCommand))
            {
                SelectedCommandInputGesture = _commandManager.GetInputGesture(SelectedCommand);
            }

            UpdateCommands();
        }

        public Command Remove { get; private set; }

        private bool OnRemoveCanExecute()
        {
            if (string.IsNullOrWhiteSpace(SelectedCommand))
            {
                return false;
            }

            return true;
        }

        private void OnRemoveExecute()
        {
            SelectedCommandInputGesture = null;
            _commandManager.UpdateInputGesture(SelectedCommand, null);
            
            UpdateCommands();
        }

        public TaskCommand Assign { get; private set; }

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

        private async Task OnAssignExecuteAsync()
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

                    messageBuilder.AppendLine(_languageService.GetString("Orchestra_AssignInputGestureUsedByFollowCommands"), selectedInputGesture);
                    messageBuilder.AppendLine();

                    foreach (var existingCommand in existingCommands)
                    {
                        messageBuilder.AppendLine("- {0}", existingCommand.Key);
                    }

                    messageBuilder.AppendLine();
                    messageBuilder.AppendLine(_languageService.GetString("Orchestra_AssignInputGestureAreYouSure"), selectedCommand);

                    if (await _messageService.ShowAsync(messageBuilder.ToString(), _languageService.GetString("Orchestra_ReplaceInputGesture"), 
                        MessageButton.YesNo) == MessageResult.No)
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

            UpdateCommands();
        }
        #endregion

        #region Methods
        protected override async Task CloseAsync()
        {
            _keyboardMappingsService.Save();

            await base.CloseAsync();
        }

        private void UpdateCommands()
        {
            var selectedCommand = SelectedCommand;

            var allCommands = _commandManager.GetCommands().OrderBy(x => x).ToList();
            if (!string.IsNullOrWhiteSpace(CommandFilter))
            {
                allCommands = allCommands.Where(x => x.IndexOf(CommandFilter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            using (Commands.SuspendChangeNotifications())
            {
                Commands.Clear();

                foreach (var command in allCommands)
                {
                    var commandInfo = _commandInfoService.GetCommandInfo(command);
                    if (!commandInfo.IsHidden)
                    {
                        Commands.Add(commandInfo);
                    }
                }
            }

            // restore selection
            if (selectedCommand != null && Commands.FirstOrDefault(x => string.Equals(x.CommandName, selectedCommand)) != null)
            {
                SelectedCommand = selectedCommand;
            }
        }

        private void OnCommandFilterChanged()
        {
            UpdateCommands();
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