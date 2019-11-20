// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskRunnerEnvironment.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.IO;

    public static class TaskRunnerEnvironment
    {
        public static readonly string CurrentLogFileName = Path.Combine(Catel.IO.Path.GetApplicationDataDirectory(), "current.log");
    }
}
