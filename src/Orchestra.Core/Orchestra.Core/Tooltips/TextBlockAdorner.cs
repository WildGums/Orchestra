// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBlockAdorner.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Tooltips
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using Catel;
    using Models;

    public class TextBlockAdorner : Adorner
    {
        #region Fields
        private readonly UIElement _adornedElement;
        private readonly Border _tooltip;
        private ArrayList _logicalChildren;
        #endregion

        #region Constructors
        public TextBlockAdorner(UIElement adornedElement, string text)
            : base(adornedElement)
        {
            Argument.IsNotNull(() => adornedElement);

            _adornedElement = adornedElement;

            var border = new Border();
            border.Background = new SolidColorBrush(Color.FromRgb(255, 255, 180));
            border.Padding = new Thickness(4, 1, 4, 1);
            border.BorderBrush = Brushes.Gray;
            border.BorderThickness = new Thickness(1);
            border.Margin = new Thickness(0, -5, -5, 0);

            var shortcut = new TextBlock {Text = text};
            shortcut.FontSize = 10;
            border.Child = shortcut;

            _tooltip = border;

            AddLogicalChild(_tooltip);
            AddVisualChild(_tooltip);
        }

        public TextBlockAdorner(UIElement uiElement, IHint hint)
            : this(uiElement, hint.Text)
        {
        }
        #endregion

        #region Properties
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (_logicalChildren == null)
                {
                    _logicalChildren = new ArrayList();
                    _logicalChildren.Add(_tooltip);
                }

                return _logicalChildren.GetEnumerator();
            }
        }
        #endregion

        #region Methods
        protected override Size MeasureOverride(Size constraint)
        {
            _tooltip.Measure(constraint);

            return _tooltip.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var location = new Point(_adornedElement.RenderSize.Width - finalSize.Width, 0);
            var rect = new Rect(location, finalSize);

            _tooltip.Arrange(rect);

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            Argument.IsNotOutOfRange("index", index, 0, 0);

            return _tooltip;
        }
        #endregion
    }
}