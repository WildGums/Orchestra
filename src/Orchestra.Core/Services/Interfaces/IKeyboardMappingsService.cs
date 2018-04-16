// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyboardMappingsService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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