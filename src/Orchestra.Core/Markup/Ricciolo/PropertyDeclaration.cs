#if NET || NETCORE

#pragma warning disable 1591 // 1591 = missing xml

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal class PropertyDeclaration
    {
        private readonly TypeDeclaration _declaringType;
        private readonly string _name;

        // Methods
        public PropertyDeclaration(string name)
        {
            this._name = name;
            this._declaringType = null;
        }

        public PropertyDeclaration(string name, TypeDeclaration declaringType)
        {
            this._name = name;
            this._declaringType = declaringType;
        }

        public override string ToString()
        {
            if (((this.DeclaringType != null) && (this.DeclaringType.Name == "XmlNamespace")) && ((this.DeclaringType.Namespace == null) && (this.DeclaringType.Assembly == null)))
            {
                if ((this.Name == null) || (this.Name.Length == 0))
                {
                    return "xmlns";
                }
                return ("xmlns:" + this.Name);
            }
            return this.Name;
        }

        // Properties
        public TypeDeclaration DeclaringType
        {
            get
            {
                return this._declaringType;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }
    }
}

#endif
