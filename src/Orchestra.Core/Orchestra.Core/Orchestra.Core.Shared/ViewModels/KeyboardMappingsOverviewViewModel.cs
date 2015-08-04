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
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Models;
    using Services;

    /// <summary>
    /// View model for keyboard mappings overview.
    /// </summary>
    public class KeyboardMappingsOverviewViewModel : ViewModelBase
    {
        private readonly ICommandManager _commandManager;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ILanguageService _languageService;
        private readonly IViewExportService _viewExportService;
        private readonly IKeyboardMappingsService _keyboardMappingsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardMappingsOverviewViewModel" /> class.
        /// </summary>
        /// <param name="commandManager">The command manager.</param>
        /// <param name="uiVisualizerService">The UI visualizer service.</param>
        /// <param name="languageService">The language service.</param>
        /// <param name="viewExportService">The view export service.</param>
        /// <param name="keyboardMappingsService">The keyboard mappings service.</param>
        public KeyboardMappingsOverviewViewModel(ICommandManager commandManager, IUIVisualizerService uiVisualizerService,
            ILanguageService languageService, IViewExportService viewExportService, IKeyboardMappingsService keyboardMappingsService)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => uiVisualizerService);
            Argument.IsNotNull(() => languageService);
            Argument.IsNotNull(() => viewExportService);
            Argument.IsNotNull(() => keyboardMappingsService);

            _commandManager = commandManager;
            _uiVisualizerService = uiVisualizerService;
            _languageService = languageService;
            _viewExportService = viewExportService;
            _keyboardMappingsService = keyboardMappingsService;

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

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

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
                var groupName = command.GetCommandGroup();
                var inputGesture = _commandManager.GetInputGesture(command);
                mappingsByGroup[groupName].Mappings.Add(new KeyboardMapping
                {
                    CommandName = command,
                    InputGesture = inputGesture
                });
            }

            var additionalKeyboardMappings = _keyboardMappingsService.AdditionalKeyboardMappings;
            foreach (var additionalKeyboardMapping in additionalKeyboardMappings)
            {
                var groupName = additionalKeyboardMapping.CommandName.GetCommandGroup();

                if (!mappingsByGroup.ContainsKey(groupName))
                {
                    mappingsByGroup[groupName] = new KeyboardMappings { GroupName = groupName };
                }

                mappingsByGroup[groupName].Mappings.Add(additionalKeyboardMapping);
            }

            if (mappingsByGroup[string.Empty].Mappings.Count == 0)
            {
                mappingsByGroup.Remove(string.Empty);
            }

            var finalMappings = new Dictionary<string, KeyboardMappings>();

            foreach (var mappingGroup in mappingsByGroup)
            {
                var keyboardMappings = new KeyboardMappings
                {
                    GroupName = mappingGroup.Key
                };

                keyboardMappings.Mappings.AddRange(mappingGroup.Value.Mappings.OrderBy(x => x.CommandName));

                finalMappings[mappingGroup.Key] = keyboardMappings;
            }

            KeyboardMappings = new List<KeyboardMappings>(finalMappings.Values.OrderBy(x => x.GroupName));
        }
    }
}