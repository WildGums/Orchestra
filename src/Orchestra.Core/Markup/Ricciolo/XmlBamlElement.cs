#if NET || NETCORE

#pragma warning disable 1591 // 1591 = missing xml

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal class XmlBamlElement : XmlBamlNode
    {
        private readonly XmlNamespaceCollection _namespaces = new XmlNamespaceCollection();

        public XmlBamlElement()
        {
        }


        public XmlBamlElement(XmlBamlElement parent)
        {
            this.Namespaces.AddRange(parent.Namespaces);
        }

        public XmlNamespaceCollection Namespaces
        {
            get { return _namespaces; }
        }

        public TypeDeclaration TypeDeclaration { get; set; }

        public override XmlNodeType NodeType
        {
            get
            {
                return XmlNodeType.Element;
            }
        }

        public long Position { get; set; }

        public override string ToString()
        {
            return String.Format("Element: {0}", TypeDeclaration.Name);
        }
    }

    internal class XmlBamlEndElement : XmlBamlElement
    {
        public XmlBamlEndElement(XmlBamlElement start)
        {
            this.TypeDeclaration = start.TypeDeclaration;
            this.Namespaces.AddRange(start.Namespaces);
        }

        public override XmlNodeType NodeType
        {
            get
            {
                return XmlNodeType.EndElement;
            }
        }

        public override string ToString()
        {
            return String.Format("EndElement: {0}", TypeDeclaration.Name);
        }
    }

    internal class KeysResourcesCollection : List<KeysResource>
    {
        public KeysResource Last
        {
            get
            {
                if (this.Count == 0)
                    return null;
                return this[this.Count - 1];
            }
        }

        public KeysResource First
        {
            get
            {
                if (this.Count == 0)
                    return null;
                return this[0];
            }
        }
    }

    internal class KeysResource
    {
        private readonly KeysTable _keys = new KeysTable();
        private readonly ArrayList _staticResources = new ArrayList();

        public KeysTable Keys
        {
            get { return _keys; }
        }

        public ArrayList StaticResources
        {
            get { return _staticResources; }
        }
    }

    internal class KeysTable
    {
        private readonly Hashtable _table = new Hashtable();

        public String this[long position]
        {
            get
            {
                return (string)this._table[position];
            }
            set
            {
                this._table[position] = value;
            }
        }

        public int Count
        {
            get { return this._table.Count; }
        }

        public void Remove(long position)
        {
            this._table.Remove(position);
        }

        public bool HasKey(long position)
        {
            return this._table.ContainsKey(position);
        }
    }
}

#endif
