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
    using System.Threading.Tasks;
    using Catel;
    using Catel.Collections;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.Windows.Input;
    using Orchestra.Services;

    public class KeyboardMappingsCustomizationViewModel : ViewModelBase
    {
        private readonly IKeyboardMappingsService _keyboardMappingsService;
        private readonly ICommandManager _commandManager;
        private readonly ILanguageService _languageService;

        public KeyboardMappingsCustomizationViewModel(IKeyboardMappingsService keyboardMappingsService, ICommandManager commandManager,
            ILanguageService languageService)
        {
            Argument.IsNotNull(() => keyboardMappingsService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => languageService);

            _keyboardMappingsService = keyboardMappingsService;
            _commandManager = commandManager;
            _languageService = languageService;

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
        private void OnAssignExecute()
        {
            SelectedCommandInputGesture = SelectedCommandNewInputGesture;
            _commandManager.UpdateInputGesture(SelectedCommand, SelectedCommandNewInputGesture);
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