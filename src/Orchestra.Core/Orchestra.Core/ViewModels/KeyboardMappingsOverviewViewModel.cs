// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappingsOverviewViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;

    /// <summary>
    /// View model for keyboard mappings overview.
    /// </summary>
    public class KeyboardMappingsOverviewViewModel : ViewModelBase
    {
        private readonly ICommandManager _commandManager;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ILanguageService _languageService;
        private readonly IViewExportService _viewExportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsOverviewViewModel"/> class.
        /// </summary>
        /// <param name="commandManager">The command manager.</param>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <param name="languageService">The language service.</param>
        /// <param name="viewExportService">The view export service.</param>
        public KeyboardMappingsOverviewViewModel(ICommandManager commandManager, IUIVisualizerService uiVisualizerService,
            ILanguageService languageService, IViewExportService viewExportService)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => languageService);
            Argument.IsNotNull(() => viewExportService);

            _commandManager = commandManager;
            _uiVisualizerService = uiVisualizerService;
            _languageService = languageService;
            _viewExportService = viewExportService;

            Print = new Command(OnPrintExecute);
            Customize = new Command(OnCustomizeExecute);
        }

        /// <summary>
        /// Gets the keyboard mappings.
        /// </summary>
        /// <value>The keyboard mappings.</value>
        public List<KeyboardMappings> KeyboardMappings { get; private set; }

        #region Commands
        /// <summary>
        /// Gets the Print command.
        /// </summary>
        public Command Print { get; private set; }

        /// <summary>
        /// Method to invoke when the Print command is executed.
        /// </summary>
        private void OnPrintExecute()
        {
            _viewExportService.Export(this);
        }

        /// <summary>
        /// Gets the Customize command.
        /// </summary>
        public Command Customize { get; private set; }

        /// <summary>
        /// Method to invoke when the Customize command is executed.
        /// </summary>
        private void OnCustomizeExecute()
        {
            _uiVisualizerService.ShowDialog<KeyboardMappingsCustomizationViewModel>();
        }
        #endregion

        protected override void Initialize()
        {
            Title = string.Format(_languageService.GetString("ShortcutsForApplication"), AssemblyHelper.GetEntryAssembly().Title());

            var mappingsByGroup = new Dictionary<string, KeyboardMappings>();
            mappingsByGroup.Add(string.Empty, new KeyboardMappings { GroupName = string.Empty });

            var commands = _commandManager.GetCommands();
            var groups = (from command in commands
                          select command.GetCommandGroup()).Distinct().OrderBy(x => x);

            foreach (var group in groups)
            {
                mappingsByGroup[group] = new KeyboardMappings { GroupName = group };
            }

            foreach (var command in commands.OrderBy(x => x.GetCommandName()))
            {
                string groupName = command.GetCommandGroup();
                var inputGesture = _commandManager.GetInputGesture(command);
                mappingsByGroup[groupName].Mappings.Add(new KeyboardMapping
                {
                    CommandName = command,
                    InputGesture = inputGesture
                });
            }

            if (mappingsByGroup[string.Empty].Mappings.Count == 0)
            {
                mappingsByGroup.Remove(string.Empty);
            }

            KeyboardMappings = new List<KeyboardMappings>(mappingsByGroup.Values);
        }
    }
}