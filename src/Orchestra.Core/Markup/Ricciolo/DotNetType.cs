#if NET || NETCORE

#pragma warning disable 1591 // 1591 = missing xml

using System;
using Catel;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal class DotNetType : MarshalByRefObject, IType
    {
        private readonly string _assemblyQualifiedName;
        private readonly Type _type;

        public DotNetType(string assemblyQualifiedName)
        {
            Argument.IsNotNull(() => assemblyQualifiedName);

            _assemblyQualifiedName = assemblyQualifiedName;
            _type = Type.GetType(assemblyQualifiedName, false, true);
        }

        #region IType Members

        public string AssemblyQualifiedName
        {
            get { return _assemblyQualifiedName; }
        }

        public bool IsSubclassOf(IType type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!(type is DotNetType)) throw new ArgumentException("type");
            if (_type == null) return false;
            return this._type.IsSubclassOf(((DotNetType)type).Type);
        }

        public bool Equals(IType type)
        {
            if (_type == null) return false;
            return this._type.Equals(((DotNetType)type).Type);
        }

        #endregion

        public Type Type
        {
            get { return _type; }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}

#endif
