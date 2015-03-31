// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThemeService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    public class ThemeService : IThemeService
    {
        public virtual bool ShouldCreateStyleForwarders()
        {
            return true;
        }
    }
}