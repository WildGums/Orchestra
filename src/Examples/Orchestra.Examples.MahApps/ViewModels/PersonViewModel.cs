// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersonViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.MahApps.ViewModels
{
    using Catel;
    using Catel.Fody;
    using Catel.MVVM;
    using Models;

    /// <summary>
    /// UserControl view model.
    /// </summary>
    public class PersonViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonViewModel"/> class.
        /// </summary>
        public PersonViewModel(Person person)
        {
            Argument.IsNotNull(() => person);

            Person = person;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return Person.ToString(); }
        }

        [Model]
        [Expose("FirstName")]
        [Expose("MiddleName")]
        [Expose("LastName")]
        public Person Person { get; private set; }
        #endregion
    }
}