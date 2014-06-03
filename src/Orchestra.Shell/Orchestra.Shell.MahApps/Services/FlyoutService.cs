// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutService.cs" company="Simulation Modelling Services">
//   Copyright (c) 2008 - 2014 Simulation Modelling Services. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FallDownMatrixManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.MVVM.Providers;
    using Catel.Windows.Threading;
    using MahApps.Metro.Controls;
    using Orchestra.Models;
    using ThemeHelper = Orchestra.ThemeHelper;

    public class FlyoutService : IFlyoutService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ITypeFactory _typeFactory;
        private readonly ICommandManager _commandManager;

        private readonly Dictionary<string, FlyoutInfo> _flyouts = new Dictionary<string, FlyoutInfo>();

        public FlyoutService(ITypeFactory typeFactory, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => typeFactory);
            Argument.IsNotNull(() => commandManager);

            _typeFactory = typeFactory;
            _commandManager = commandManager;
        }

        public IEnumerable<Flyout> GetFlyouts()
        {
            return (from flyout in _flyouts.Values
                    select flyout.Flyout);
        }

        public void AddFlyout(string name, Type viewType, Position position, UnloadBehavior unloadBehavior = UnloadBehavior.SaveAndCloseViewModel)
        {
            Argument.IsNotNullOrWhitespace("name", name);
            Argument.IsNotNull("viewType", viewType);

            Log.Info("Adding flyout '{0}' with view type '{1}'", name, viewType.FullName);

            var content = (UIElement)_typeFactory.CreateInstance(viewType);

            var flyout = new Flyout();
            flyout.Theme = FlyoutTheme.Adapt;
            flyout.Position = position;

            var flyoutInfo = new FlyoutInfo(flyout, content);

            flyout.SetBinding(Flyout.HeaderProperty, new Binding("ViewModel.Title") { Source = content });

            ((ICompositeCommand)_commandManager.GetCommand("Close")).RegisterAction(() => { flyout.IsOpen = false; });

            flyout.IsOpenChanged += (sender, e) =>
            {
                if (!flyout.IsOpen)
                {
                    var vmContainer = flyout.Content as IViewModelContainer;
                    if (vmContainer != null)
                    {
                        var vm = vmContainer.ViewModel;
                        if (vm != null)
                        {
                            switch (unloadBehavior)
                            {
                                case UnloadBehavior.CloseViewModel:
                                    vm.CloseViewModel(null);
                                    break;

                                case UnloadBehavior.SaveAndCloseViewModel:
                                    vm.SaveAndCloseViewModel();
                                    break;

                                case UnloadBehavior.CancelAndCloseViewModel:
                                    vm.CancelAndCloseViewModel();
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException("unloadBehavior");
                            }
                        }
                    }

                    flyout.Content = null;
                    flyout.DataContext = null;
                }
            };

            _flyouts[name] = flyoutInfo;
        }

        public void ShowFlyout(string name, object dataContext)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            if (!_flyouts.ContainsKey(name))
            {
                Log.ErrorAndThrowException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyoutInfo = _flyouts[name];
            var flyout = flyoutInfo.Flyout;

            var flyoutsControl = flyout.Parent as FlyoutsControl;
            if (flyoutsControl != null)
            {
                flyoutsControl.BorderThickness = new Thickness(1);
                flyoutsControl.BorderBrush = ThemeHelper.GetAccentColorBrush();
            }

            flyout.Content = flyoutInfo.Content;
            flyout.DataContext = dataContext;
            flyout.Dispatcher.BeginInvoke(() => flyout.IsOpen = true);
        }

        public void HideFlyout(string name)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            if (!_flyouts.ContainsKey(name))
            {
                Log.ErrorAndThrowException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyoutInfo = _flyouts[name];
            var flyout = flyoutInfo.Flyout;

            flyout.IsOpen = false;
        }
    }
}