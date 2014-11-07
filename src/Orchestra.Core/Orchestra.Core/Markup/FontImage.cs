// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageBinding.cs" company="Catel development team">
//   Copyright (c) 2008 - 2014 Catel development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Markup
{
    using System;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;
    using Catel.Logging;
    using Catel.Windows.Markup;
    using Brush = System.Windows.Media.Brush;
    using FontFamily = System.Windows.Media.FontFamily;
    using FontStyle = System.Windows.FontStyle;
    using Point = System.Windows.Point;

    /// <summary>
    /// Markup extension that can show a font as image.
    /// </summary>
    /// <remarks>
    /// Original idea comes from http://www.codeproject.com/Tips/634540/Using-Font-Icons
    /// </remarks>
    public class FontImage : UpdatableMarkupExtension
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Constructors
        static FontImage()
        {
            DefaultFontFamily = new FontFamily("Segoe UI Symbol");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontImage"/> class.
        /// </summary>
        public FontImage()
        {
            FontFamily = DefaultFontFamily;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontImage" /> class.
        /// </summary>
        /// <param name="itemName">Name of the resource.</param>
        public FontImage(string itemName)
            : this()
        {
            ItemName = itemName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the default name of the font.
        /// </summary>
        /// <value>The default name of the font.</value>
        public static FontFamily DefaultFontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>The font family.</value>
        public FontFamily FontFamily { get; set; }

        /// <summary>
        /// Gets or sets the font item name.
        /// </summary>
        /// <value>The font item name.</value>
        [ConstructorArgument("itemName")]
        public string ItemName { get; set; }
        #endregion

        /// <summary>
        /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
        /// </summary>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        protected override object ProvideDynamicValue()
        {
            var fontFamily = FontFamily;
            if (fontFamily == null)
            {
                Log.Error("No font is available, make sure to set both FontUri and FontFamily");
                return null;
            }

            // TODO: Consider caching
            var glyph = CreateGlyph(ItemName, fontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, Brushes.Black);
            return glyph;
        }

        private static ImageSource CreateGlyph(string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, Brush foreBrush)
        {
            if (fontFamily != null && !String.IsNullOrEmpty(text))
            {
                var typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);

                GlyphTypeface glyphTypeface;
                if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                {
                    Log.ErrorAndThrowException<InvalidOperationException>("No glyph type face found");
                }

                var glyphIndexes = new ushort[text.Length];
                var advanceWidths = new double[text.Length];
                for (var i = 0; i < text.Length; i++)
                {
                    ushort glyphIndex;

                    try
                    {
                        glyphIndex = glyphTypeface.CharacterToGlyphMap[text[i]];
                    }
                    catch (Exception)
                    {
                        glyphIndex = 42;
                    }

                    glyphIndexes[i] = glyphIndex;

                    var width = glyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
                    advanceWidths[i] = width;
                }

                try
                {
                    var gr = new GlyphRun(glyphTypeface, 0, false, 1.0, glyphIndexes, new Point(0, 0), advanceWidths, null, null, null, null, null, null);
                    var glyphRunDrawing = new GlyphRunDrawing(foreBrush, gr);

                    return new DrawingImage(glyphRunDrawing);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in generating Glyphrun");
                }
            }

            return null;
        }
    }
}