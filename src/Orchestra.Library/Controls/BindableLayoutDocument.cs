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
    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Bindable implementation of the <see cref="LayoutDocument"/> which automatically binds
    /// the title.
    /// </summary>
    public class BindableLayoutDocument : LayoutDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindableLayoutDocument" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="tag">The tag.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="view" /> is <c>null</c>.</exception>
        public BindableLayoutDocument(FrameworkElement view, object tag = null)
        {
            Argument.IsNotNull(() => view);
            Argument.IsNotNull(() => view.DataContext);
            Argument.IsOfType(() => view.DataContext, typeof(IViewModel));

            CanFloat = false;
            Title = ((IViewModel)view.DataContext).Title;
            Content = view;

            ((IViewModel)view.DataContext).PropertyChanged += OnLayoutDocumentViewModelPropertyChanged();

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