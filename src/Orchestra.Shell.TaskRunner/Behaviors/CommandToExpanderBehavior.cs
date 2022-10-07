namespace Orchestra.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Catel.MVVM;
    using Catel.Windows.Interactivity;

    public class CommandToExpanderBehavior : BehaviorBase<Expander>
    {
        public ICommand? Command
        {
            get { return (ICommand?)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand),
            typeof(CommandToExpanderBehavior), new PropertyMetadata((sender, e) => ((CommandToExpanderBehavior)sender).OnCommandChanged(e)));

        private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldCommand = e.OldValue as ICatelCommand;
            if (oldCommand is not null)
            {
                oldCommand.Executed -= OnCommandExecuted;
            }

            var newCommand = e.NewValue as ICatelCommand;
            if (newCommand is not null)
            {
                newCommand.Executed += OnCommandExecuted;
            }
        }

        private void OnCommandExecuted(object? sender, CommandExecutedEventArgs e)
        {
            var expander = AssociatedObject;
            if (expander is not null)
            {
                expander.SetCurrentValue(Expander.IsExpandedProperty, !expander.IsExpanded);
            }
        }
    }
}
