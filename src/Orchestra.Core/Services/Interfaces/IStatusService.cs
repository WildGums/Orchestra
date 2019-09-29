// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStatusService.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    [ObsoleteEx(ReplacementTypeOrMember = "Orc.Controls.Services.IStatusRepresenter", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]
    public interface IStatusRepresenter : Orc.Controls.Services.IStatusRepresenter
    {
    }

    public interface IStatusService
    {
        void Initialize(Orc.Controls.Services.IStatusRepresenter statusRepresenter);
        #region Methods
        [ObsoleteEx(ReplacementTypeOrMember = "Initialize(Orc.Controls.Services.IStatusRepresenter)", TreatAsErrorFromVersion = "5.0", RemoveInVersion = "6.0")]
        void Initialize(IStatusRepresenter statusRepresenter);
        void UpdateStatus(string status);
        #endregion
    }
}
