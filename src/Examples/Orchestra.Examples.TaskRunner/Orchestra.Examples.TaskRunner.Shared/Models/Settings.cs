// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.TaskRunner.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Catel.Data;

    public class Settings : ModelBase
    {
        public Settings()
        {
            CurrentTime = DateTime.Now;
            HorizonStart = DateTime.Now.Date;
            HorizonEnd = DateTime.Now.AddDays(30);
        }

        [DefaultValue(@"./Output")]
        public string OutputDirectory { get; set; }

        [DefaultValue(@"./Data")]
        public string WorkingDirectory { get; set; }

        public DateTime? CurrentTime { get; set; }

        public DateTime? HorizonStart { get; set; }

        public DateTime? HorizonEnd { get; set; }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            base.ValidateFields(validationResults);

            if (!HorizonStart.HasValue)
            {
                validationResults.Add(FieldValidationResult.CreateError("HorizonStart", "Horizon start date is required"));
            }

            if (!HorizonEnd.HasValue)
            {
                validationResults.Add(FieldValidationResult.CreateError("HorizonEnd", "Horizon end date is required"));
            }

            if (!CurrentTime.HasValue)
            {
                validationResults.Add(FieldValidationResult.CreateError("CurrentTime", "Current time is required"));
            }
            else
            {
                if (HorizonStart.HasValue && HorizonEnd.HasValue)
                {
                    if (CurrentTime.Value < HorizonStart.Value || CurrentTime.Value > HorizonEnd.Value)
                    {
                        validationResults.Add(FieldValidationResult.CreateError("CurrentTime", "Current time must be a date inside the horizon range"));
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(OutputDirectory))
            {
                validationResults.Add(FieldValidationResult.CreateError("OutputDirectory", "Output directory is required"));
            }

            if (string.IsNullOrWhiteSpace(WorkingDirectory))
            {
                validationResults.Add(FieldValidationResult.CreateError("WorkingDirectory", "Working directory is required"));
            }
        }
    }
}