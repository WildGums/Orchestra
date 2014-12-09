// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenFilePickerViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.ViewModels
{
    using System.IO;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class OpenFilePickerViewModel : ViewModelBase
    {
        #region Fields
        private readonly IProcessService _processService;
        private readonly IOpenFileService _selectFileService;
        #endregion

        #region Constructors
        public OpenFilePickerViewModel(IOpenFileService selectFileService, IProcessService processService)
        {
            Argument.IsNotNull(() => selectFileService);
            Argument.IsNotNull(() => processService);

            _selectFileService = selectFileService;
            _processService = processService;

            OpenDirectory = new Command(OnOpenDirectoryExecute, OnOpenDirectoryCanExecute);
            SelectFile = new Command(OnSelectFileExecute);
        }
        #endregion

        #region Properties
        public double LabelWidth { get; set; }

        public string LabelText { get; set; }

        public string SelectedFile { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the OpenDirectory command.
        /// </summary>
        public Command OpenDirectory { get; private set; }

        /// <summary>
        /// Method to check whether the OpenDirectory command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnOpenDirectoryCanExecute()
        {
            if (string.IsNullOrWhiteSpace(SelectedFile))
            {
                return false;
            }

            var directory = Directory.GetParent(SelectedFile);
            // Don't allow users to write text that they can "invoke" via our software
            if (!directory.Exists)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method to invoke when the OpenDirectory command is executed.
        /// </summary>
        private void OnOpenDirectoryExecute()
        {
            var directory = Directory.GetParent(SelectedFile);
            _processService.StartProcess(directory.FullName);
        }

        /// <summary>
        /// Gets the SelectFile command.
        /// </summary>
        public Command SelectFile { get; private set; }

        /// <summary>
        /// Method to invoke when the SelectFile command is executed.
        /// </summary>
        private void OnSelectFileExecute()
        {
            if (!string.IsNullOrEmpty(SelectedFile))
            {
                _selectFileService.InitialDirectory = Path.GetFullPath(SelectedFile);
            }

            if (_selectFileService.DetermineFile())
            {
                SelectedFile = _selectFileService.FileName;
            }
        }
        #endregion
    }
}