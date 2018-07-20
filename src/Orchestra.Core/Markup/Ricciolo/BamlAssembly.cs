#if NET

#pragma warning disable 1591 // 1591 = missing xml

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    internal class BamlAssembly : MarshalByRefObject
    {
        private readonly string _filePath;
        private readonly Assembly _assembly;
        private BamlFileList _bamlFile;

        public BamlAssembly(Assembly assembly)
        {
            _assembly = assembly;
            _filePath = assembly.CodeBase;

            ReadBaml();
        }

        public BamlAssembly(string filePath)
        {
            this._filePath = Path.GetFullPath(filePath);
            this._assembly = Assembly.LoadFile(this.FilePath);
            if (String.Compare(this.Assembly.CodeBase, this.FilePath, true) != 0)
            {
                throw new ArgumentException("Cannot load filePath because Assembly is already loaded", "filePath");
            }

            ReadBaml();
        }

        private void ReadBaml()
        {
            // Get available names
            var resources = this.Assembly.GetManifestResourceNames();
            foreach (var res in resources)
            {
                // Solo le risorse
                if (String.Compare(Path.GetExtension(res), ".resources", true) != 0)
                {
                    continue;
                }

                // Get stream
                using (var stream = this.Assembly.GetManifestResourceStream(res))
                {
                    try
                    {
                        var reader = new ResourceReader(stream);
                        foreach (DictionaryEntry entry in reader)
                        {
                            if (String.Compare(Path.GetExtension(entry.Key.ToString()), ".baml", true) == 0 && entry.Value is Stream)
                            {
                                var bm = new BamlFile(GetAssemblyResourceUri(entry.Key.ToString()), (Stream)entry.Value);
                                this.BamlFiles.Add(bm);
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                        // ignore
                    }
                }
            }
        }

        private Uri GetAssemblyResourceUri(string resourceName)
        {
            var asm = this.Assembly.GetName();
            var data = asm.GetPublicKeyToken();
            var token = new StringBuilder(data.Length * 2);
            for (var x = 0; x < data.Length; x++)
            {
                token.Append(data[x].ToString("x", System.Globalization.CultureInfo.InvariantCulture));
            }

            return new Uri(String.Format(@"{0};V{1};{2};component\{3}", asm.Name, asm.Version, token, Path.ChangeExtension(resourceName, ".xaml")), UriKind.RelativeOrAbsolute);
        }

        public string FilePath
        {
            get { return _filePath; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }

        public BamlFileList BamlFiles
        {
            get
            {
                if (_bamlFile == null)
                {
                    _bamlFile = new BamlFileList();
                }

                return _bamlFile;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    [Serializable]
    internal class BamlFileList : Collection<BamlFile>
    {

    }
}

#endif
