#if NET

#pragma warning disable 1591 // 1591 = missing xml

using System;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    /// <summary>
    /// Rappresenta la mappatura tra namespace XML e namespace CLR con relativo assembly
    /// </summary>
    internal class XmlPIMapping
    {
        private readonly short _assemblyId;
        private readonly string _clrNamespace;
        private static readonly XmlPIMapping _default = new XmlPIMapping(PresentationNamespace, 0, String.Empty);

        public const string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml";
        public const string PresentationNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        public const string PresentationOptionsNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation/options";
        public const string McNamespace = "http://schemas.openxmlformats.org/markup-compatibility/2006";

        public XmlPIMapping(string xmlNamespace, short assemblyId, string clrNamespace)
        {
            XmlNamespace = xmlNamespace;
            _assemblyId = assemblyId;
            _clrNamespace = clrNamespace;
        }

        /// <summary>
        /// Restituisce o imposta il namespace XML
        /// </summary>
        public string XmlNamespace { get; set; }

        /// <summary>
        /// Restituisce l'id dell'assembly
        /// </summary>
        public short AssemblyId
        {
            get { return _assemblyId; }
        }

        /// <summary>
        /// Restituisce il namespace clr
        /// </summary>
        public string ClrNamespace
        {
            get { return _clrNamespace; }
        }

        /// <summary>
        /// Restituisce il mapping di default di WPF
        /// </summary>
        public static XmlPIMapping Presentation
        {
            get { return _default; }
        }
    }
}

#endif
