﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogControlService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Catel.Logging;

    /// <summary>
    /// Log control service.
    /// </summary>
    public interface ILogControlService
    {
        LogEvent SelectedLevel { get; set; }

        void Clear();
    }
}
