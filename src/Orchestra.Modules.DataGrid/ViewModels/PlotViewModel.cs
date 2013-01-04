// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Modules.DataGrid.ViewModels
{
    using System.Collections.Generic;
    using Catel;
    using Catel.Data;
    using Orchestra.ViewModels;

    /// <summary>
    /// Plot view model.
    /// </summary>
    public class PlotViewModel : ViewModelBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotViewModel"/> class.
        /// </summary>
        public PlotViewModel(IEnumerable<string> columns)
        {
            Argument.IsNotNull("columns", columns);

            AvailableColumns = new List<string>(columns);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title
        {
            get { return "Plot data"; }
        }

        public List<string> AvailableColumns { get; private set; }

        public string XAxis { get; set; }

        public string YAxis { get; set; }
        #endregion

        #region Commands
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets
        #endregion

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(XAxis))
            {
                validationResults.Add(FieldValidationResult.CreateError("XAxis", "X axis is required"));
            }

            if (string.IsNullOrWhiteSpace(YAxis))
            {
                validationResults.Add(FieldValidationResult.CreateError("YAxis", "Y axis is required"));
            }
        }

        protected override void ValidateBusinessRules(List<IBusinessRuleValidationResult> validationResults)
        {
            if (!string.IsNullOrWhiteSpace(XAxis) && !string.IsNullOrWhiteSpace(YAxis))
            {
                if (ObjectHelper.AreEqual(XAxis, YAxis))
                {
                    validationResults.Add(BusinessRuleValidationResult.CreateError("X and Y axis cannot be the same"));
                }
            }
        }
    }
}