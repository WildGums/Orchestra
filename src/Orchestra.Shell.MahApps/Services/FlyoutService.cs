// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.Windows.Threading;
    using MahApps.Metro.Controls;
    using Models;

    public class FlyoutService : IFlyoutService
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICommandManager _commandManager;
        private readonly Dictionary<string, FlyoutInfo> _flyouts = new Dictionary<string, FlyoutInfo>();
        private readonly ITypeFactory _typeFactory;
        #endregion

        #region Constructors
        public FlyoutService(ITypeFactory typeFactory, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => commandManager);

            _typeFactory = typeFactory;
            _commandManager = commandManager;
        }
        #endregion

        #region Methods
        public IEnumerable<Flyout> GetFlyouts()
        {
            return (from flyout in _flyouts.Values
                    select flyout.Flyout);
        }

        public void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel, FlyoutTheme flyoutTheme = FlyoutTheme.Adapt)
        {
            Argument.IsNotNullOrWhitespace(() => name);
            Argument.IsNotNull(() => viewType);

            Log.Info("Adding flyout '{0}' with view type '{1}'", name, viewType.FullName);

            var content = (UIElement)_typeFactory.CreateInstance(viewType);

            var flyout = new Flyout();
            flyout.Theme = flyoutTheme;
            flyout.Position = position;

            var flyoutInfo = new FlyoutInfo(flyout, content);

            // See https://github.com/WildGums/Orchestra/issues/278, we cannot bind this, use workaround for now, see workaround below as well!!!
            //flyout.SetBinding(Flyout.HeaderProperty, new Binding("ViewModel.Title") { Source = content });

            ((ICompositeCommand)_commandManager.GetCommand("Close")).RegisterAction(() => { flyout.IsOpen = false; });

            // ViewModelChanged handler (Workaround for https://github.com/WildGums/Orchestra/issues/278)
            var vmContainer = content as IViewModelContainer;
            EventHandler<EventArgs> viewModelChangedHandler = null;
            viewModelChangedHandler = (sender, e) =>
            {
                var title = vmContainer?.ViewModel?.Title;
                if (!string.IsNullOrWhiteSpace(title))
                {
                    flyout.Dispatcher.BeginInvoke(() =>
                    {
                        flyout.SetCurrentValue(Flyout.HeaderProperty, title);
                    });
                }
            };

            vmContainer.ViewModelChanged += viewModelChangedHandler;

            // IsOpenChanged handler
            RoutedEventHandler isOpenHandler = null;
#pragma warning disable AvoidAsyncVoid
            isOpenHandler = async (sender, e) =>
#pragma warning restore AvoidAsyncVoid
            {
                var vmContainer = flyout.Content as IViewModelContainer;
                var vm = vmContainer?.ViewModel;

                if (!flyout.IsOpen)
                {
                    if (vm is not null)
                    {
                        switch (unloadBehavior)
                        {
                            case UnloadBehavior.CloseViewModel:
                                await vm.CloseViewModelAsync(null);
                                break;

                            case UnloadBehavior.SaveAndCloseViewModel:
                                await vm.SaveAndCloseViewModelAsync();
                                break;

                            case UnloadBehavior.CancelAndCloseViewModel:
                                await vm.CancelAndCloseViewModelAsync();
                                break;

                            default:
                                throw new ArgumentOutOfRangeException(nameof(unloadBehavior));
                        }
                    }

                    flyout.Content = null;
                    flyout.DataContext = null;

                    flyout.IsOpenChanged -= isOpenHandler;
                    vmContainer.ViewModelChanged -= viewModelChangedHandler;
                }
            };

            flyout.IsOpenChanged += isOpenHandler;

            _flyouts[name] = flyoutInfo;
        }


        public void ShowFlyout(string name, object dataContext)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            if (!_flyouts.ContainsKey(name))
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyoutInfo = _flyouts[name];
            var flyout = flyoutInfo.Flyout;

            flyout.Dispatcher.BeginInvoke(() =>
            {
                var flyoutsControl = flyout.Parent as FlyoutsControl;
                if (flyoutsControl is not null)
                {
                    flyoutsControl.SetCurrentValue(System.Windows.Controls.Control.BorderThicknessProperty, new Thickness(1));
                    flyoutsControl.SetCurrentValue(System.Windows.Controls.Control.BorderBrushProperty, Orc.Theming.ThemeManager.Current.GetAccentColorBrush());
                }

                var isOpen = flyout.IsOpen;
                if (!isOpen)
                {
                    flyout.SetCurrentValue(System.Windows.Controls.ContentControl.ContentProperty, flyoutInfo.Content);
                }

                flyout.SetValue(FrameworkElement.DataContextProperty, dataContext);

                if (!isOpen)
                {
                    flyout.SetCurrentValue(Flyout.IsOpenProperty, true);
                }
            });
        }

        public void HideFlyout(string name)
        {
            Argument.IsNotNullOrWhitespace(() => name);

            if (!_flyouts.ContainsKey(name))
            {
                throw Log.ErrorAndCreateException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyoutInfo = _flyouts[name];
            var flyout = flyoutInfo.Flyout;

            flyout.BeginInvoke(() =>
            {
                flyout.SetCurrentValue(Flyout.IsOpenProperty, false);
            });
        }
        #endregion
    }
}
