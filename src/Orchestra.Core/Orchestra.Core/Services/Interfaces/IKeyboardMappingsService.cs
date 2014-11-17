// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyboardMappingsService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public interface IKeyboardMappingsService
    {
        void Load();
        void Save();
        void Reset();
    }
}