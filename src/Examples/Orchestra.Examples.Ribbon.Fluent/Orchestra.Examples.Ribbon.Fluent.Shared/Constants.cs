// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Examples.Ribbon
{
    using System.Windows.Input;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public static class Commands
    {
        public static class Application
        {
            public const string Exit = "Application.Exit";
            public static readonly InputGesture ExitInputGesture = new InputGesture(Key.F4, ModifierKeys.Alt);

            public const string About = "Application.About";
            public static readonly InputGesture AboutInputGesture = new InputGesture(Key.F1);
        }

        public static class Demo
        {
            public const string LongOperation = "Demo.LongOperation";
            public static readonly InputGesture LongOperationInputGesture = null;

            public const string ShowMessageBox = "Demo.ShowMessageBox";
            public static readonly InputGesture ShowMessageBoxInputGesture = null;

            public const string Hidden = "Demo.Hidden";
            public static readonly InputGesture HiddenInputGesture = new InputGesture(Key.H, ModifierKeys.Control);
        }
    }

    public static class Settings
    {
        public static class Application
        {
            public static class General
            {
                public const string SomeSetting = "General.SomeSetting";
                public const bool SomeSettingDefaultValue = true;
            }
        }
    }
}