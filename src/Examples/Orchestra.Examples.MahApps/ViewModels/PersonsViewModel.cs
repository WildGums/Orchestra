// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonsViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.MVVM;
    using FallDownMatrixManager.Services;
    using Models;

    public class PersonsViewModel : ViewModelBase
    {
        private readonly IFlyoutService _flyoutService;

        #region Constructors
        public PersonsViewModel(IFlyoutService flyoutService)
        {
            Argument.IsNotNull(() => flyoutService);

            _flyoutService = flyoutService;

            Persons = new ObservableCollection<Person>();
            Persons.Add(new Person
            {
                FirstName = "John",
                LastName = "Doe"
            });

            Edit = new Command(OnEditExecute, OnEditCanExecute);
        }
        #endregion

        #region Properties
        public ObservableCollection<Person> Persons { get; private set; }

        public Person SelectedPerson { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the Edit command.
        /// </summary>
        public Command Edit { get; private set; }

        /// <summary>
        /// Method to check whether the Edit command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool OnEditCanExecute()
        {
            return SelectedPerson != null;
        }

        /// <summary>
        /// Method to invoke when the Edit command is executed.
        /// </summary>
        private void OnEditExecute()
        {
            _flyoutService.ShowFlyout(ExampleEnvironment.PersonFlyoutName, SelectedPerson);
        }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            SelectedPerson = Persons.FirstOrDefault();
        }
        #endregion
    }
}