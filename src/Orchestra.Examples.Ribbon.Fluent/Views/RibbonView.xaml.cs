namespace Orchestra.Examples.Ribbon.Views
{
    using Catel.IoC;
    using Microsoft.Extensions.DependencyInjection;

    public partial class RibbonView 
    {
        partial void OnInitializedComponent()
        {
            ribbon.AddAboutButton(IoCContainer.ServiceProvider.GetRequiredService<IAboutService>());
        }

        protected override void OnViewModelChanged()
        {
            base.OnViewModelChanged();

#pragma warning disable WPF0041
            backstageTabControl.DataContext = ViewModel;
#pragma warning restore WPF0041
        }
    }
}
