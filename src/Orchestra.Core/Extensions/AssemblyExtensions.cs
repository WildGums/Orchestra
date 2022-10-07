namespace Orchestra
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Media.Imaging;

    public static class AssemblyExtensions
    {
        public static Icon? ExtractAssemblyIcon(this Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            return IconHelper.ExtractIconFromFile(assembly.Location);
        }

        public static BitmapImage? ExtractLargestIcon(this Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            return IconHelper.ExtractLargestIconFromFile(assembly.Location);
        }
    }
}
