[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v5.0", FrameworkDisplayName="")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Controls")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Views")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Windows")]
[assembly: System.Windows.Markup.XmlnsPrefix("http://schemas.wildgums.com/orchestra", "orchestra")]
[assembly: System.Windows.ThemeInfo(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]
public static class LoadAssembliesOnStartup { }
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orchestra.Controls
{
    public class RibbonBackstageButton : System.Windows.Controls.Button
    {
        public static readonly System.Windows.DependencyProperty IsDefinitiveProperty;
        public static readonly System.Windows.DependencyProperty ShowBorderProperty;
        public RibbonBackstageButton() { }
        public bool IsDefinitive { get; set; }
        public bool ShowBorder { get; set; }
        protected override void OnClick() { }
    }
    public class RibbonBackstageTabControl : System.Windows.Controls.TabControl
    {
        public RibbonBackstageTabControl() { }
    }
    public class RibbonBackstageTabItem : System.Windows.Controls.TabItem
    {
        public static readonly System.Windows.DependencyProperty HeaderTextProperty;
        public static readonly System.Windows.DependencyProperty IconProperty;
        public RibbonBackstageTabItem() { }
        public string HeaderText { get; set; }
        public System.Windows.Media.ImageSource Icon { get; set; }
    }
    public class RibbonBackstageTabItemHeader : System.Windows.Controls.ContentControl
    {
        public static readonly System.Windows.DependencyProperty HeaderTextProperty;
        public static readonly System.Windows.DependencyProperty HeaderTextStyleKeyProperty;
        public static readonly System.Windows.DependencyProperty IconProperty;
        public static readonly System.Windows.DependencyProperty KeepIconSizeWithoutIconProperty;
        public RibbonBackstageTabItemHeader() { }
        public string HeaderText { get; set; }
        public string HeaderTextStyleKey { get; set; }
        public System.Windows.Media.ImageSource Icon { get; set; }
        public bool KeepIconSizeWithoutIcon { get; set; }
    }
}
namespace Orchestra
{
    public static class RibbonExtensions
    {
        public static void AddAboutButton(this Fluent.Ribbon ribbon) { }
        public static Fluent.Button AddRibbonButton(this Fluent.Ribbon ribbon, System.Uri imageUri, System.Action action) { }
        public static Fluent.Button AddRibbonButton(this Fluent.Ribbon ribbon, System.Windows.Media.ImageSource imageSource, System.Action action) { }
    }
}
namespace Orchestra.Services
{
    public class ApplicationInitializationServiceBase : Orchestra.Services.IApplicationInitializationService
    {
        public ApplicationInitializationServiceBase() { }
        public virtual bool ShowChangelog { get; }
        public virtual bool ShowShell { get; }
        public virtual bool ShowSplashScreen { get; }
        public virtual System.Threading.Tasks.Task InitializeAfterCreatingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeAfterShowingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeBeforeCreatingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeBeforeShowingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeBeforeShowingSplashScreenAsync() { }
        protected virtual void InitializeLogging() { }
        protected virtual System.Threading.Tasks.Task ShowChangelogAsync() { }
        protected static System.Threading.Tasks.Task RunAndWaitAsync(params System.Func<>[] actions) { }
    }
    public interface IApplicationInitializationService
    {
        bool ShowShell { get; }
        bool ShowSplashScreen { get; }
        System.Threading.Tasks.Task InitializeAfterCreatingShellAsync();
        System.Threading.Tasks.Task InitializeAfterShowingShellAsync();
        System.Threading.Tasks.Task InitializeBeforeCreatingShellAsync();
        System.Threading.Tasks.Task InitializeBeforeShowingShellAsync();
        System.Threading.Tasks.Task InitializeBeforeShowingSplashScreenAsync();
    }
    public interface IRibbonService : Orchestra.Services.IShellContentService
    {
        System.Windows.FrameworkElement GetRibbon();
    }
    public interface IShellConfigurationService
    {
        bool DeferValidationUntilFirstSaveCall { get; set; }
    }
    public interface IShellContentService
    {
        System.Windows.FrameworkElement GetMainView();
        System.Windows.FrameworkElement GetStatusBar();
    }
    public interface IShellService
    {
        Orchestra.Views.IShell Shell { get; }
        System.Threading.Tasks.Task<TShell> CreateAsync<TShell>()
            where TShell :  class, Orchestra.Views.IShell;
    }
    public class ShellConfigurationService : Orchestra.Services.IShellConfigurationService
    {
        public ShellConfigurationService() { }
        public virtual bool DeferValidationUntilFirstSaveCall { get; set; }
    }
    public class ShellService : Orchestra.Services.IShellService
    {
        public ShellService(Catel.IoC.ITypeFactory typeFactory, Orchestra.Services.IKeyboardMappingsService keyboardMappingsService, Catel.MVVM.ICommandManager commandManager, Orchestra.Services.ISplashScreenService splashScreenService, Orchestra.Services.IEnsureStartupService ensureStartupService, Orchestra.Services.IApplicationInitializationService applicationInitializationService, Catel.IoC.IDependencyResolver dependencyResolver, Catel.IoC.IServiceLocator serviceLocator) { }
        public Orchestra.Views.IShell Shell { get; }
        public System.Threading.Tasks.Task<TShell> CreateAsync<TShell>()
            where TShell :  class, Orchestra.Views.IShell { }
    }
    public class XamlResourceService : Orchestra.Services.IXamlResourceService
    {
        public XamlResourceService() { }
        public virtual System.Collections.Generic.IEnumerable<System.Windows.ResourceDictionary> GetApplicationResourceDictionaries() { }
        protected virtual System.Windows.ResourceDictionary GetResourceDictionaryFromAssembly(System.Reflection.Assembly assembly) { }
        protected virtual System.Uri GetResourceDictionaryUriFromAssembly(System.Reflection.Assembly assembly) { }
    }
}
namespace Orchestra.ViewModels
{
    public class ShellViewModel : Catel.MVVM.ViewModelBase
    {
        public ShellViewModel(Orchestra.Services.IShellConfigurationService shellConfigurationService) { }
    }
}
namespace Orchestra.Views
{
    public interface IShell
    {
        void Show();
    }
    public class ShellWindow : Orchestra.Windows.RibbonWindow, Orchestra.Views.IShell, System.Windows.Markup.IComponentConnector
    {
        public ShellWindow() { }
        public void InitializeComponent() { }
    }
}
namespace Orchestra.Windows
{
    public class RibbonWindow : Fluent.RibbonWindow, Catel.MVVM.IViewModelContainer, Catel.MVVM.Views.IDataWindow, Catel.MVVM.Views.IView, System.ComponentModel.INotifyPropertyChanged
    {
        public RibbonWindow() { }
        public RibbonWindow(Catel.MVVM.IViewModel viewModel) { }
        public Catel.MVVM.IViewModel ViewModel { get; }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public event System.EventHandler<System.EventArgs> ViewModelChanged;
    }
}