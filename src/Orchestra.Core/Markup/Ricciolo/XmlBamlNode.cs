#if NET || NETCORE

#pragma warning disable 1591 // 1591 = missing xml

using System.Xml;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal class XmlBamlNode
    {
        public virtual XmlNodeType NodeType
        {
            get { return XmlNodeType.None;}
        }
    }
}

#endif
