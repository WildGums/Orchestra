// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IKeyboardMappingsServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System.Threading.Tasks;

    using Catel;
    using Catel.Threading;

    public static class IKeyboardMappingsServiceExtensions
    {
        [ObsoleteEx(Message = "Wrap in TaskHelper.Run yourself if async is needed", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public static Task LoadAsync(this IKeyboardMappingsService keyboardMappingsService)
        {
            Argument.IsNotNull(() => keyboardMappingsService);

            return TaskHelper.Run(() => keyboardMappingsService.Load(), true);
        }

        [ObsoleteEx(Message = "Wrap in TaskHelper.Run yourself if async is needed", TreatAsErrorFromVersion = "2.0", RemoveInVersion = "3.0")]
        public static Task SaveAsync(this IKeyboardMappingsService keyboardMappingsService)
        {
            Argument.IsNotNull(() => keyboardMappingsService);

            return TaskHelper.Run(() => keyboardMappingsService.Save(), true);
        }
    }
}