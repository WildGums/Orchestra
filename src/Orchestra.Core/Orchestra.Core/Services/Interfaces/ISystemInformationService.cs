// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystemInformationService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2015 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Collections.Generic;

    public interface ISystemInformationService
    {
        IEnumerable<string> GetSystemInfo();
    }
}