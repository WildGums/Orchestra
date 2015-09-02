// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyboardMappingsService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IKeyboardMappingsService
    {
        void Load();
        void Save();
        void Reset();
        List<KeyboardMapping> AdditionalKeyboardMappings { get; }
    }
}