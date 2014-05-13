// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.TaskRunner.ViewModels
{
    using System.Collections.Generic;
    using Catel;
    using Catel.Data;
    using Catel.Fody;
    using Catel.Logging;
    using Catel.MVVM;
    using Models;

    public class SettingsViewModel : ViewModelBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors
        public SettingsViewModel(Settings settings)
        {
            Argument.IsNotNull(() => settings);

            Settings = settings;
        }
        #endregion

        #region Properties
        [Model]
        [Expose("OutputDirectory")]
        [Expose("WorkingDirectory")]
        [Expose("CurrentTime")]
        [Expose("HorizonStart")]
        [Expose("HorizonEnd")]
        public Settings Settings { get; private set; }
        #endregion
    }
}