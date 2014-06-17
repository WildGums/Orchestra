// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandToSelectedTabBehavior.cs" company="Simulation Modelling Services">
//   Copyright (c) 2008 - 2014 Simulation Modelling Services. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.MVVM;
    using Catel.Windows.Interactivity;

    public class CommandToExpanderBehavior : BehaviorBase<Expander>
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand),
            typeof(CommandToExpanderBehavior), new PropertyMetadata((sender, e) => ((CommandToExpanderBehavior)sender).OnCommandChanged(e)));

        private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldCommand = e.OldValue as ICatelCommand;
            if (oldCommand != null)
            {
                oldCommand.Executed -= OnCommandExecuted;
            }

            var newCommand = e.NewValue as ICatelCommand;
            if (newCommand != null)
            {
                newCommand.Executed += OnCommandExecuted;
            }
        }

        private void OnCommandExecuted(object sender, CommandExecutedEventArgs e)
        {
            var expander = AssociatedObject;
            if (expander != null)
            {
                expander.IsExpanded = !expander.IsExpanded;
            }
        }
    }
}