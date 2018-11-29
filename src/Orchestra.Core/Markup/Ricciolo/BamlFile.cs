#if NET || NETCORE

#pragma warning disable 1591 // 1591 = missing xml

using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using Catel;

namespace Orchestra.StylesExplorer.MarkupReflection
{
    /// <summary>
    /// Rappresenta un singole file Baml all'interno di un assembly
    /// </summary>
    internal class BamlFile : Component
    {
        private readonly Uri _uri;
        private readonly Stream _stream;

        public BamlFile(Uri uri, Stream stream)
        {
            Argument.IsNotNull(() => uri);
            Argument.IsNotNull(() => stream);

            _uri = uri;
            _stream = stream;
        }

        /// <summary>
        /// Carica il Baml attraverso il motore di WPF con Application.LoadComponent
        /// </summary>
        /// <returns></returns>
        public object LoadContent()
        {
            try
            {
                return Application.LoadComponent(Uri);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Invalid baml file.", e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                Stream.Dispose();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Restituisce lo stream originale contenente il Baml
        /// </summary>
        public Stream Stream
        {
            get { return _stream; }
        }

        /// <summary>
        /// Restituisce l'indirizzo secondo lo schema pack://
        /// </summary>
        public Uri Uri
        {
            get { return _uri; }
        }

    }
}

#endif
