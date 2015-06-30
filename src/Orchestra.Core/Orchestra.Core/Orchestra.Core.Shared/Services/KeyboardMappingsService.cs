// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardMappingsService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
    using Orchestra.Models;
    using Path = Catel.IO.Path;

    public class KeyboardMappingsService : IKeyboardMappingsService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICommandManager _commandManager;
        private readonly IXmlSerializer _xmlSerializer;

        private readonly string _fileName;

        public KeyboardMappingsService(ICommandManager commandManager, IXmlSerializer xmlSerializer)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => xmlSerializer);

            _commandManager = commandManager;
            _xmlSerializer = xmlSerializer;
            _fileName = Path.Combine(Path.GetApplicationDataDirectory(), "keyboardmappings.xml");

            AdditionalKeyboardMappings = new List<KeyboardMapping>();
        }

        public List<KeyboardMapping> AdditionalKeyboardMappings { get; private set; } 

        public void Load()
        {
            Log.Info("Loading keyboard mappings");

            try
            {
                if (!File.Exists(_fileName))
                {
                    Log.Warning("Keyboard mapping file not found at '{0}'", _fileName);
                    return;
                }

                using (var fileStream = File.Open(_fileName, FileMode.Open, FileAccess.Read))
                {
                    var keyboardMappings = _xmlSerializer.Deserialize(typeof (KeyboardMappings), fileStream) as KeyboardMappings;
                    if (keyboardMappings != null)
                    {
                        foreach (var keyboardMapping in keyboardMappings.Mappings)
                        {
                            Log.Debug("Updating keyboard mapping for command '{0}' to '{1}'", keyboardMapping.CommandName, keyboardMapping.InputGesture);

                            if (!_commandManager.IsCommandCreated(keyboardMapping.CommandName))
                            {
                                Log.Warning("Command '{0}' is not created in the CommandManager, cannot update input gesture", keyboardMapping.CommandName);
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
            Log.Info("Saving keyboard mappings");

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

                using (var fileStream = File.Open(_fileName, FileMode.Create, FileAccess.Write))
                {
                    _xmlSerializer.Serialize(keyboardMappings, fileStream);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save the keyboard mappings");
            }
        }

        public void Reset()
        {
            Log.Info("Resetting keyboard mappings");

            _commandManager.ResetInputGestures();
        }
    }
}