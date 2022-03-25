// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.IO;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xml;
    using Catel;
    using Catel.Windows;

    public static class DependencyObjectExtensions
    {
        public static T Clone<T>(this T source)
            where T : DependencyObject
        {
            Argument.IsNotNull(() => source);

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
        public static System.Windows.Window GetParentWindow(this DependencyObject visualObject)
        {
            return visualObject?.FindLogicalOrVisualAncestorByType<System.Windows.Window>();
        }
    }
}
