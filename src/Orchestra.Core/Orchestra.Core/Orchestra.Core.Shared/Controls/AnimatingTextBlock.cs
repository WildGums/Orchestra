// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatingTextBlock.cs" company="CatenaLogic development team">
//   Copyright (c) 2008 - 2014 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using Catel.Windows.Threading;
    using Services;

    /// <summary>
    /// Animating text block.
    /// </summary>
    public class AnimatingTextBlock : UserControl, IStatusRepresenter
    {
        private int _currentIndex = 0;
        private Grid _contentGrid;
        private readonly List<TextBlock> _textBlocks = new List<TextBlock>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatingTextBlock"/> class.
        /// </summary>
        public AnimatingTextBlock()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;

            ApplyTemplate();
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// The text property.
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(AnimatingTextBlock), 
            new PropertyMetadata(string.Empty, (sender, e) => ((AnimatingTextBlock)sender).OnTextChanged()));

        /// <summary>
        /// Gets or sets the hide storyboard.
        /// </summary>
        /// <value>The hide storyboard.</value>
        public Storyboard HideStoryboard
        {
            get { return (Storyboard)GetValue(HideStoryboardProperty); }
            set { SetValue(HideStoryboardProperty, value); }
        }

        /// <summary>
        /// The hide storyboard property.
        /// </summary>
        public static readonly DependencyProperty HideStoryboardProperty =
            DependencyProperty.Register("HideStoryboard", typeof(Storyboard), typeof(AnimatingTextBlock), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the show storyboard.
        /// </summary>
        /// <value>The show storyboard.</value>
        public Storyboard ShowStoryboard
        {
            get { return (Storyboard)GetValue(ShowStoryboardProperty); }
            set { SetValue(ShowStoryboardProperty, value); }
        }

        /// <summary>
        /// The show storyboard property.
        /// </summary>
        public static readonly DependencyProperty ShowStoryboardProperty =
            DependencyProperty.Register("ShowStoryboard", typeof(Storyboard), typeof(AnimatingTextBlock), new PropertyMetadata(null));

        private void OnTextChanged()
        {
            if (_contentGrid == null)
            {
                return;
            }

            var textBlockToHide = _contentGrid.Children[_currentIndex];
            if (textBlockToHide.Opacity == 0.0d)
            {
                OnHideComplete();
                return;
            }

            var hideStoryboard = HideStoryboard;
            if (hideStoryboard != null)
            {
                hideStoryboard.Stop();
                Storyboard.SetTarget(hideStoryboard, textBlockToHide);

                hideStoryboard.Completed += OnHideStoryboardComplete;
                hideStoryboard.Begin();
            }
            else
            {
                textBlockToHide.Opacity = 0d;
                OnHideComplete();
            }
        }

        private void OnHideStoryboardComplete(object sender, EventArgs e)
        {
            var storyBoard = sender as Storyboard;
            if (storyBoard != null)
            {
                storyBoard.Completed -= OnHideStoryboardComplete;
            }

            OnHideComplete();
        }

        private void OnHideComplete()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            _currentIndex++;
            if (_currentIndex >= _contentGrid.Children.Count)
            {
                _currentIndex = 0;
            }

            var textBlockToShow = (TextBlock)_contentGrid.Children[_currentIndex];
            textBlockToShow.Text = Text;

            var showStoryboard = ShowStoryboard;
            if (showStoryboard != null)
            {
                showStoryboard.Stop();
                Storyboard.SetTarget(showStoryboard, textBlockToShow);

                showStoryboard.Completed += OnShowStoryboardComplete;
                showStoryboard.Begin();
            }
            else
            {
                textBlockToShow.Opacity = 1d;
            }
        }

        private void OnShowStoryboardComplete(object sender, EventArgs e)
        {
            var storyBoard = sender as Storyboard;
            if (storyBoard != null)
            {
                storyBoard.Completed -= OnShowStoryboardComplete;
            }
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeControl();
        }

        private void InitializeControl()
        {
            _contentGrid = CreateContent();
            Content = _contentGrid;
        }

        private Grid CreateContent()
        {
            var grid = new Grid();

            var renderTransform = RenderTransform;
            RenderTransform = null;

            for (int i = 0; i < 2; i++)
            {
                var textBlock = new TextBlock();
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Opacity = 0d;
                textBlock.RenderTransform = renderTransform;

                _textBlocks.Add(textBlock);

                grid.Children.Add(textBlock);
            }

            return grid;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="status">The status.</param>
        public void UpdateStatus(string status)
        {
            Dispatcher.BeginInvoke(() => Text = status);
        }
    }
}