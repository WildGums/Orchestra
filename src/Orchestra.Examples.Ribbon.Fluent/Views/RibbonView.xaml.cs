namespace Orchestra.Examples.Ribbon.Views;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Catel.IoC;
using Microsoft.Extensions.DependencyInjection;

public partial class RibbonView 
{
    partial void OnInitializedComponent()
    {
        ribbon.AddAboutButton(IoCContainer.ServiceProvider.GetRequiredService<IAboutService>());
    }

    private void OnLargeDialogClick(object sender, RoutedEventArgs e)
    {
        var closeButton = new Button
        {
            Content = "Close",
            IsCancel = true,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(16),
            MinWidth = 96
        };

        DockPanel.SetDock(closeButton, Dock.Bottom);

        var content = new DockPanel();
        content.Children.Add(closeButton);
        content.Children.Add(new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = new TextBlock
            {
                Margin = new Thickness(16),
                Text = string.Join(Environment.NewLine, Enumerable.Range(1, 200).Select(row => $"Content row {row}"))
            }
        });

        var dialog = new Window
        {
            Title = "Large auto-sized dialog",
            Owner = Window.GetWindow(this),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            SizeToContent = SizeToContent.Height,
            Width = 640,
            Content = content
        };

        dialog.SetMaximumHeight();
        dialog.ShowDialog();
    }

    protected override void OnViewModelChanged()
    {
        base.OnViewModelChanged();

#pragma warning disable WPF0041
        backstageTabControl.DataContext = ViewModel;
#pragma warning restore WPF0041
    }
}
