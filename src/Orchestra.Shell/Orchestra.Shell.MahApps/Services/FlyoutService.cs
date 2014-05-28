// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlyoutService.cs" company="Simulation Modelling Services">
//   Copyright (c) 2008 - 2014 Simulation Modelling Services. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace FallDownMatrixManager.Services
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Data;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using MahApps.Metro.Controls;
    using ThemeHelper = Orchestra.ThemeHelper;

    public class FlyoutService : IFlyoutService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ITypeFactory _typeFactory;
        private readonly Dictionary<string, Flyout> _flyouts = new Dictionary<string, Flyout>();

        public FlyoutService(ITypeFactory typeFactory)
        {
            Argument.IsNotNull(() => typeFactory);

            _typeFactory = typeFactory;
        }

        public IEnumerable<Flyout> GetFlyouts()
        {
            return _flyouts.Values;
        }

        public void AddFlyout(string name, Type viewType, Position position)
        {
            Argument.IsNotNullOrWhitespace("name", name);
            Argument.IsNotNull("viewType", viewType);

            Log.Info("Adding flyout '{0}' with view type '{1}'", name, viewType.FullName);

            var flyout = new Flyout();
            flyout.Content = _typeFactory.CreateInstance(viewType);
            flyout.Position = position;
            flyout.BorderThickness = new Thickness(1);
            flyout.BorderBrush = ThemeHelper.GetAccentColorBrush();
            flyout.SetBinding(Flyout.HeaderProperty, new Binding("ViewModel.Title") { Source = flyout.Content });

            _flyouts[name] = flyout;
        }

        public void ShowFlyout(string name, object dataContext)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            if (!_flyouts.ContainsKey(name))
            {
                Log.ErrorAndThrowException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyout = _flyouts[name];
            flyout.DataContext = dataContext;
            flyout.IsOpen = true;
        }

        public void HideFlyout(string name)
        {
            Argument.IsNotNullOrWhitespace("name", name);

            if (!_flyouts.ContainsKey(name))
            {
                Log.ErrorAndThrowException<InvalidOperationException>("Flyout '{0}' is not added yet", name);
            }

            var flyout = _flyouts[name];
            flyout.IsOpen = false;
            flyout.DataContext = null;
        }
    }
}