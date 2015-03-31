// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyboardMappingsServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;

    using Catel;

    public static class IKeyboardMappingsServiceExtensions
    {
        public static async Task LoadAsync(this IKeyboardMappingsService keyboardMappingsService)
        {
            Argument.IsNotNull(() => keyboardMappingsService);

            await Task.Factory.StartNew(() => keyboardMappingsService.Load());
        }

        public static async Task SaveAsync(this IKeyboardMappingsService keyboardMappingsService)
        {
            Argument.IsNotNull(() => keyboardMappingsService);

            await Task.Factory.StartNew(() => keyboardMappingsService.Save());
        }
    }
}