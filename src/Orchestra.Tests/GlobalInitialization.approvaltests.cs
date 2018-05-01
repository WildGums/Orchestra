// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalInitialization.approvaltests.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using ApprovalTests.Reporters;

#if DEBUG
[assembly: UseReporter(typeof(BeyondCompare4Reporter), typeof(DiffReporter), typeof(AllFailingTestsClipboardReporter))]
#else
[assembly: UseReporter(typeof(DiffReporter))]
#endif

public static class TargetFrameworkResolver
{
    public const string Current =

#if NET45
            "NET45"
#elif NET46
            "NET46"
#elif NET47
            "NET47"
#else
            "Unknown"
#endif
        ;
}