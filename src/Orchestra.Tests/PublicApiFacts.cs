// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tests
{
    using System.Runtime.CompilerServices;
    using ApiApprover;
    using NUnit.Framework;
    using Services;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orchestra_Core_HasNoBreakingChanges()
        {
            var assembly = typeof(AboutService).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orchestra_Shell_MahApps_HasNoBreakingChanges()
        {
            var assembly = typeof(MahAppsHelper).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orchestra_Shell_Ribbon_Fluent_HasNoBreakingChanges()
        {
            var assembly = typeof(RibbonExtensions).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}