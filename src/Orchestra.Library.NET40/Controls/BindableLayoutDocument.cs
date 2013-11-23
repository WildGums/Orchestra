// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindableLayoutDocument.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2013 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orchestra.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using Catel;
    using Catel.MVVM;
    using Orchestra.Views;
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Bindable implementation of the <see cref="LayoutDocument"/> which automatically binds
    /// the title.
    /// </summary>
    public class BindableLayoutDocument : LayoutAnchorable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindableLayoutDocument" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="canFloat">if set to <c>true</c>, the document can float.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="view" /> is <c>null</c>.</exception>
        public BindableLayoutDocument(FrameworkElement view, object tag = null, bool canFloat = false)
        {
            Argument.IsNotNull("view", view);
            Argument.IsNotNull("view.DataContext", view.DataContext);
            Argument.IsOfType("view.DataContext", view.DataContext, typeof(IViewModel));

            var vm = ((IViewModel) view.DataContext);

            CanFloat = canFloat;
            Title = vm.Title;
            Content = view;

            vm.PropertyChanged += OnLayoutDocumentViewModelPropertyChanged();

            view.Tag = tag;
        }

        private PropertyChangedEventHandler OnLayoutDocumentViewModelPropertyChanged()
        {
            return (s, e) =>
            {
                if (string.Equals(e.PropertyName, "Title"))
                {
                    Title = ((IViewModel)s).Title;
                }
            };
        }

        /// <summary>
        /// Raises the <c>Closing</c> event.
        /// </summary>
        /// <param name="e">The <see cref="CancelEventArgs"/> instance containing the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            var view = Content as IDocumentView;
            if (view != null)
            {
                if (!view.CloseDocument())
                {
                    e.Cancel = true;
                }
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// Called when the document is closed.
        /// </summary>
        protected override void OnClosed()
        {
            var view = Content as FrameworkElement;
            if (view == null)
            {
                return;
            }

            var vm = view.DataContext as IViewModel;
            if (vm == null)
            {
                return;
            }

            vm.PropertyChanged -= OnLayoutDocumentViewModelPropertyChanged();

            base.OnClosed();
        }
    }
}