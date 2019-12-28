// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappingsService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Runtime.Serialization.Xml;
    using Catel.Services;
    using Orc.FileSystem;
    using Orchestra.Models;
    using Path = Catel.IO.Path;

    public class KeyboardMappingsService : IKeyboardMappingsService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICommandManager _commandManager;
        private readonly IXmlSerializer _xmlSerializer;
        private readonly IFileService _fileService;
        private readonly IAppDataService _appDataService;
        private readonly string _fileName;

        public KeyboardMappingsService(ICommandManager commandManager, IXmlSerializer xmlSerializer, 
            IFileService fileService, IAppDataService appDataService)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => xmlSerializer);
            Argument.IsNotNull(() => fileService);
            Argument.IsNotNull(() => appDataService);

            _commandManager = commandManager;
            _xmlSerializer = xmlSerializer;
            _fileService = fileService;
            _appDataService = appDataService;

            _fileName = Path.Combine(appDataService.GetApplicationDataDirectory(Catel.IO.ApplicationDataTarget.UserRoaming), "keyboardmappings.xml");

            AdditionalKeyboardMappings = new List<KeyboardMapping>();
        }

        public List<KeyboardMapping> AdditionalKeyboardMappings { get; private set; } 

        public void Load()
        {
            Log.Debug("Loading keyboard mappings");

            try
            {
                if (!_fileService.Exists(_fileName))
                {
                    Log.Debug("Keyboard mapping file not found at '{0}'", _fileName);
                    return;
                }

                using (var fileStream = _fileService.OpenRead(_fileName))
                {
                    var keyboardMappings = _xmlSerializer.Deserialize(typeof (KeyboardMappings), fileStream, null) as KeyboardMappings;
                    if (keyboardMappings != null)
                    {
                        foreach (var keyboardMapping in keyboardMappings.Mappings)
                        {
                            Log.Debug("Updating keyboard mapping for command '{0}' to '{1}'", keyboardMapping.CommandName, keyboardMapping.InputGesture);

                            if (!_commandManager.IsCommandCreated(keyboardMapping.CommandName))
                            {
                                Log.Debug("Command '{0}' is not created in the CommandManager, cannot update input gesture", keyboardMapping.CommandName);
                                continue;
                            }

                            _commandManager.UpdateInputGesture(keyboardMapping.CommandName, keyboardMapping.InputGesture);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to load the keyboard mappings");
            }
        }

        public void Save()
        {
            Log.Debug("Saving keyboard mappings");

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

                using (var fileStream = _fileService.Create(_fileName))
                {
                    _xmlSerializer.Serialize(keyboardMappings, fileStream, null);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save the keyboard mappings");
            }
        }

        public void Reset()
        {
            Log.Debug("Resetting keyboard mappings");

            _commandManager.ResetInputGestures();
        }
    }
}
