// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonsViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Models;
    using Orchestra.Services;

    public class PersonsViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IFlyoutService _flyoutService;
        private readonly IMessageService _messageService;
        private readonly IDispatcherService _dispatcherService;

        #region Constructors
        public PersonsViewModel(IFlyoutService flyoutService, IMessageService messageService, IDispatcherService dispatcherService)
        {
            Argument.IsNotNull(() => flyoutService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => dispatcherService);

            _flyoutService = flyoutService;
            _messageService = messageService;
            _dispatcherService = dispatcherService;

            Persons = new ObservableCollection<Person>();
            Persons.Add(new Person
            {
                FirstName = "John",
                LastName = "Doe"
            });

            Add = new Command(OnAddExecute);
            Edit = new Command(OnEditExecute, OnEditCanExecute);
            Remove = new TaskCommand(OnRemoveExecuteAsync, OnRemoveCanExecute);
        }
        #endregion

        #region Properties
        public ObservableCollection<Person> Persons { get; private set; }

        public Person SelectedPerson { get; set; }
        #endregion

        #region Commands
        public Command Add { get; private set; }

        private void OnAddExecute()
        {
            Log.Info("Adding new person");

            var person = new Person();

            Persons.Add(person);
            SelectedPerson = person;

            _flyoutService.ShowFlyout(ExampleEnvironment.PersonFlyoutName, SelectedPerson);
        }

        public Command Edit { get; private set; }

        private bool OnEditCanExecute()
        {
            return SelectedPerson != null;
        }

        private void OnEditExecute()
        {
            var selectedPerson = SelectedPerson;

            Log.Info("Editing person '{0}'", selectedPerson);

            _flyoutService.ShowFlyout(ExampleEnvironment.PersonFlyoutName, selectedPerson);
        }

        public TaskCommand Remove { get; private set; }

        private bool OnRemoveCanExecute()
        {
            return SelectedPerson != null;
        }

        private async Task OnRemoveExecuteAsync()
        {
            var selectedPerson = SelectedPerson;

            if (await _messageService.ShowAsync(string.Format("Are you sure you want to remove person '{0}'?", selectedPerson), "Are you sure?", MessageButton.YesNo, MessageImage.Question) == MessageResult.Yes)
            {
                Log.Info("Removing person '{0}'", selectedPerson);

                _dispatcherService.Invoke(() =>
                {
                    Persons.Remove(selectedPerson);
                    SelectedPerson = Persons.FirstOrDefault();
                }, true);
            }
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            SelectedPerson = Persons.FirstOrDefault();
        }
        #endregion
    }
}