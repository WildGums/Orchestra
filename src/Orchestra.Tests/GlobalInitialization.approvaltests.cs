// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalInitialization.approvaltests.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using ApprovalTests.Reporters;

#if DEBUG
[assembly: UseReporter(typeof(BeyondCompareReporter))]
#else
[assembly: UseReporter(typeof(DiffReporter))]
#endif
