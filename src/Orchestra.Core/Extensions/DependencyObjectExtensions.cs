namespace Orchestra
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xml;
    using Catel.Windows;

    public static class DependencyObjectExtensions
    {
        public static T Clone<T>(this T source)
            where T : DependencyObject
        {
            ArgumentNullException.ThrowIfNull(source);

            var objXaml = XamlWriter.Save(source);

            using (var stringReader = new StringReader(objXaml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    var target = (T)XamlReader.Load(xmlReader);
                    return target;
                }
            }
        }

        /// <summary>
        /// Get the parent window for this visual object or null when not exists.
        /// </summary>
        /// <param name="visualObject">Reference to visual object.</param>
        /// <returns>Reference to partent window or null when not exists.</returns>
        public static System.Windows.Window? GetParentWindow(this DependencyObject visualObject)
        {
            return visualObject?.FindLogicalOrVisualAncestorByType<System.Windows.Window>();
        }
    }
}
