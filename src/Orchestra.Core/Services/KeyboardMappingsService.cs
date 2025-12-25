namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Catel.Services;
    using Microsoft.Extensions.Logging;
    using Orc.FileSystem;

    public class KeyboardMappingsService : IKeyboardMappingsService
    {
        private readonly ILogger<KeyboardMappingsService> _logger;
        private readonly ICommandManager _commandManager;
        //private readonly IXmlSerializer _xmlSerializer;
        private readonly IFileService _fileService;
        private readonly IAppDataService _appDataService;
        private readonly string _fileName;

        public KeyboardMappingsService(ILogger<KeyboardMappingsService> logger, 
            ICommandManager commandManager, /* IXmlSerializer xmlSerializer, */
            IFileService fileService, IAppDataService appDataService)
        {
            _logger = logger;
            _commandManager = commandManager;
            //_xmlSerializer = xmlSerializer;
            _fileService = fileService;
            _appDataService = appDataService;

            _fileName = Path.Combine(appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "keyboardmappings.xml");

            AdditionalKeyboardMappings = new List<KeyboardMapping>();
        }

        public List<KeyboardMapping> AdditionalKeyboardMappings { get; private set; } 

        public async Task LoadAsync()
        {
            _logger.LogDebug("Loading keyboard mappings");

            try
            {
                if (!_fileService.Exists(_fileName))
                {
                    _logger.LogDebug("Keyboard mapping file not found at '{0}'", _fileName);
                    return;
                }

                //using (var fileStream = _fileService.OpenRead(_fileName))
                //{
                //    var keyboardMappings = _xmlSerializer.Deserialize(typeof (KeyboardMappings), fileStream, null) as KeyboardMappings;
                //    if (keyboardMappings is not null)
                //    {
                //        foreach (var keyboardMapping in keyboardMappings.Mappings)
                //        {
                //            _logger.LogDebug("Updating keyboard mapping for command '{0}' to '{1}'", keyboardMapping.CommandName, keyboardMapping.InputGesture);

                //            if (!_commandManager.IsCommandCreated(keyboardMapping.CommandName))
                //            {
                //                _logger.LogDebug("Command '{0}' is not created in the CommandManager, cannot update input gesture", keyboardMapping.CommandName);
                //                continue;
                //            }

                //            _commandManager.UpdateInputGesture(keyboardMapping.CommandName, keyboardMapping.InputGesture);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load the keyboard mappings");
            }
        }

        public async Task SaveAsync()
        {
            _logger.LogDebug("Saving keyboard mappings");

            try
            {
                var keyboardMappings = new KeyboardMappings();
                foreach (var command in _commandManager.GetCommands())
                {
                    var keyboardMapping = new KeyboardMapping();
                    keyboardMapping.CommandName = command;
                    keyboardMapping.InputGesture = _commandManager.GetInputGesture(command);

                    keyboardMappings.Mappings.Add(keyboardMapping);
                }

                //using (var fileStream = _fileService.Create(_fileName))
                //{
                //    _xmlSerializer.Serialize(keyboardMappings, fileStream, null);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save the keyboard mappings");
            }
        }

        public async Task ResetAsync()
        {
            _logger.LogDebug("Resetting keyboard mappings");

            _commandManager.ResetInputGestures();
        }
    }
}
