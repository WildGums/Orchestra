#if NET

#pragma warning disable 1591 // 1591 = missing xml

using System.Xml;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal class XmlBamlProperty : XmlBamlNode
    {
        private readonly PropertyType _propertyType;

        public XmlBamlProperty(PropertyType propertyType)
        {
            _propertyType = propertyType;
        }

        public XmlBamlProperty(PropertyType propertyType, PropertyDeclaration propertyDeclaration)
        {
            PropertyDeclaration = propertyDeclaration;
            _propertyType = propertyType;
        }

        public override string ToString()
        {
            return PropertyDeclaration.Name;
        }

        public PropertyDeclaration PropertyDeclaration { get; set; }

        public PropertyType PropertyType
        {
            get
            {
                return _propertyType;
            }
        }

        public object Value { get; set; }

        public override XmlNodeType NodeType
        {
            get
            {
                return XmlNodeType.Attribute;
            }
        }
    }

    internal enum PropertyType
    {
        Key,
        Value,
        Content,
        List,
        Dictionary,
        Complex
    }
}

#endif
