#if NET || NETCORE

#pragma warning disable 1591 // 1591 = missing xml

using System.Collections.Generic;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal class XmlNamespace
    {
        private readonly string _prefix;
        private readonly string _namespace;

        public XmlNamespace(string prefix, string ns)
        {
            _prefix = prefix;
            _namespace = ns;
        }

        public string Prefix
        {
            get { return _prefix; }
        }

        public string Namespace
        {
            get { return _namespace; }
        }

        public override bool Equals(object obj)
        {
            if (obj is XmlNamespace xmlNamespace)
            {
                return (xmlNamespace.Prefix.Equals(Prefix) && xmlNamespace.Namespace.Equals(Namespace));
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _prefix.GetHashCode() + _namespace.GetHashCode() >> 20;
        }
    }

    internal class XmlNamespaceCollection : List<XmlNamespace>
    {}
}

#endif
