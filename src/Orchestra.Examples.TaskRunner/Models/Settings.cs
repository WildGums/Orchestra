namespace Orchestra.Examples.TaskRunner.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Catel;
using Catel.Data;

public class Settings : ValidatableModelBase
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
            validationResults.Add(FieldValidationResult.CreateError("HorizonStart", LanguageHelper.GetRequiredString("Orchestra_Examples_TaskRunner_Settings_HorizonStartRequired")));
        }

        if (!HorizonEnd.HasValue)
        {
            validationResults.Add(FieldValidationResult.CreateError("HorizonEnd", LanguageHelper.GetRequiredString("Orchestra_Examples_TaskRunner_Settings_HorizonEndRequired")));
        }

        if (!CurrentTime.HasValue)
        {
            validationResults.Add(FieldValidationResult.CreateError("CurrentTime", LanguageHelper.GetRequiredString("Orchestra_Examples_TaskRunner_Settings_CurrentTimeRequired")));
        }
        else
        {
            if (HorizonStart.HasValue && HorizonEnd.HasValue)
            {
                if (CurrentTime.Value < HorizonStart.Value || CurrentTime.Value > HorizonEnd.Value)
                {
                    validationResults.Add(FieldValidationResult.CreateError("CurrentTime", LanguageHelper.GetRequiredString("Orchestra_Examples_TaskRunner_Settings_CurrentTimeInsideHorizon")));
                }
            }
        }

        if (string.IsNullOrWhiteSpace(OutputDirectory))
        {
            validationResults.Add(FieldValidationResult.CreateError("OutputDirectory", LanguageHelper.GetRequiredString("Orchestra_Examples_TaskRunner_Settings_OutputDirectoryRequired")));
        }

        if (string.IsNullOrWhiteSpace(WorkingDirectory))
        {
            validationResults.Add(FieldValidationResult.CreateError("WorkingDirectory", LanguageHelper.GetRequiredString("Orchestra_Examples_TaskRunner_Settings_WorkingDirectoryRequired")));
        }
    }
}