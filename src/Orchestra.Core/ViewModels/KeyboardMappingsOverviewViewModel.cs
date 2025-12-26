namespace Orchestra.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Reflection;
    using Catel.Services;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// View model for keyboard mappings overview.
    /// </summary>
    public class KeyboardMappingsOverviewViewModel : ViewModelBase
    {
        private readonly ILogger<KeyboardMappingsOverviewViewModel> _logger;
        private readonly ICommandManager _commandManager;
        private readonly ICommandInfoService _commandInfoService;
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly ILanguageService _languageService;
        private readonly IKeyboardMappingsService _keyboardMappingsService;

        public KeyboardMappingsOverviewViewModel(ILogger<KeyboardMappingsOverviewViewModel> logger, IServiceProvider serviceProvider, 
            ICommandManager commandManager, ICommandInfoService commandInfoService, IUIVisualizerService uiVisualizerService,
            ILanguageService languageService, IKeyboardMappingsService keyboardMappingsService)
        {
            _logger = logger;
            _commandManager = commandManager;
            _commandInfoService = commandInfoService;
            _uiVisualizerService = uiVisualizerService;
            _languageService = languageService;
            _keyboardMappingsService = keyboardMappingsService;

            ValidateUsingDataAnnotations = false;

            Customize = new TaskCommand(serviceProvider, OnCustomizeExecuteAsync);
            KeyboardMappings = new List<KeyboardMappings>();
        }

        /// <summary>
        /// Gets the keyboard mappings.
        /// </summary>
        /// <value>The keyboard mappings.</value>
        public List<KeyboardMappings> KeyboardMappings { get; private set; }

        /// <summary>
        /// Gets the Customize command.
        /// </summary>
        public TaskCommand Customize { get; private set; }

        /// <summary>
        /// Method to invoke when the Customize command is executed.
        /// </summary>
        private Task OnCustomizeExecuteAsync()
        {
            return _uiVisualizerService.ShowDialogAsync<KeyboardMappingsCustomizationViewModel>();
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Title = string.Format(_languageService.GetRequiredString("Orchestra_ShortcutsForApplication"), AssemblyHelper.GetRequiredEntryAssembly().Title());

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
                var commandInfo = _commandInfoService.GetCommandInfo(command);
                if (commandInfo.IsHidden)
                {
                    _logger.LogDebug("Command '{0}' is hidden, not showing in keyboard mappings overview", command);
                    continue;
                }

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
