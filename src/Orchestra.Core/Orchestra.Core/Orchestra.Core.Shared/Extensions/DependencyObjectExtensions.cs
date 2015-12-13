// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DependencyObjectExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.IO;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xml;
    using Catel;

    public static class DependencyObjectExtensions
    {
        public static T Clone<T>(this T source)
            where T : DependencyObject
        {
            Argument.IsNotNull(() => source);

            var objXaml = XamlWriter.Save(source);
            var stringReader = new StringReader(objXaml);
            var xmlReader = XmlReader.Create(stringReader);

            var target = (T)XamlReader.Load(xmlReader);
            return target;
        }
    }
}