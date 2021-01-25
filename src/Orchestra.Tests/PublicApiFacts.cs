// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tests
{
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using ApprovalTests;
    using ApprovalTests.Namers;
    using NUnit.Framework;
    using PublicApiGenerator;
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
            var assembly = typeof(MahAppsAboutService).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orchestra_Shell_Ribbon_Fluent_HasNoBreakingChanges()
        {
            var assembly = typeof(RibbonExtensions).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        internal static class PublicApiApprover
        {
            public static void ApprovePublicApi(Assembly assembly)
            {
                var publicApi = ApiGenerator.GeneratePublicApi(assembly, new ApiGeneratorOptions());
                var writer = new ApprovalTextWriter(publicApi, "cs");
                var approvalNamer = new AssemblyPathNamer(assembly.Location);
                Approvals.Verify(writer, approvalNamer, Approvals.GetReporter());
            }
        }

        internal class AssemblyPathNamer : UnitTestFrameworkNamer
        {
            private readonly string _name;

            public AssemblyPathNamer(string assemblyPath)
            {
                _name = Path.GetFileNameWithoutExtension(assemblyPath);

            }
            public override string Name
            {
                get { return _name; }
            }
        }
    }
}
