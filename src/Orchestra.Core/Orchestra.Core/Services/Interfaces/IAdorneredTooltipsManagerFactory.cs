// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdorneredTooltipsManagerFactory.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Windows.Documents;

    public interface IAdorneredTooltipsManagerFactory
    {
        IAdorneredTooltipsManager Create(AdornerLayer adornerLayer);
    }
}