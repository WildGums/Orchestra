// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Windows;
    using Catel;
    using Catel.Reflection;

    public static class ApplicationExtensions
    {
        public static void ApplyTheme(this Application application, bool createStyleForwarders = true)
        {
            Argument.IsNotNull(() => application);

            ThemeHelper.EnsureApplicationThemes(application.GetType().GetAssemblyEx(), createStyleForwarders);
        }
    }
}