// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAdorneredTooltipsManagerFactory.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
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