// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using Orc.Controls.Services;

    public interface IStatusService
    {
        void Initialize(IStatusRepresenter statusRepresenter);
        void UpdateStatus(string status);
    }
}
