// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Media.Imaging;
    using Catel;

    public static class AssemblyExtensions
    {
        public static Icon ExtractAssemblyIcon(this Assembly assembly)
        {
            Argument.IsNotNull(() => assembly);

            return IconHelper.ExtractIconFromFile(assembly.Location);
        }

        public static BitmapImage ExtractLargestIcon(this Assembly assembly)
        {
            Argument.IsNotNull(() => assembly);

            return IconHelper.ExtractLargestIconFromFile(assembly.Location);
        }
    }
}