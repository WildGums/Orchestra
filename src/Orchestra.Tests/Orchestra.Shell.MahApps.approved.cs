[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orchestra", "Orchestra.Views")]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.wildgums.com/orchestra", "Orchestra.Windows")]
[assembly: System.Windows.Markup.XmlnsPrefixAttribute("http://schemas.wildgums.com/orchestra", "orchestra")]
[assembly: System.Windows.ThemeInfoAttribute(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]
public class static LoadAssembliesOnStartup { }
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orchestra
{
    public class static MahAppsExtensions
    {
        public static void Close(this MahApps.Metro.Controls.Dialogs.BaseMetroDialog dialog, System.Windows.Window parentDialogWindow = null) { }
        public static MahApps.Metro.Controls.MetroWindow GetMainWindow(this System.Windows.Application application) { }
        public static void Show(this MahApps.Metro.Controls.Dialogs.BaseMetroDialog dialog) { }
        public static void ShowModal(this MahApps.Metro.Controls.Dialogs.BaseMetroDialog dialog) { }
    }
    public class static MahAppsHelper
    {
        public static void ApplyTheme() { }
    }
    public class static WindowCommandHelper
    {
        public static System.Windows.Controls.Button CreateWindowCommandButton(string style, string label) { }
        public static System.Windows.Shapes.Rectangle CreateWindowCommandRectangle(System.Windows.Controls.Button parentButton, string style) { }
    }
}
namespace Orchestra.Models
{
    public class FlyoutInfo
    {
        public FlyoutInfo(MahApps.Metro.Controls.Flyout flyout, object content) { }
        public object Content { get; set; }
        public MahApps.Metro.Controls.Flyout Flyout { get; set; }
    }
}
namespace Orchestra.Services
{
    public class ApplicationInitializationServiceBase : Orchestra.Services.IApplicationInitializationService
    {
        public ApplicationInitializationServiceBase() { }
        public virtual bool ShowShell { get; }
        public virtual bool ShowSplashScreen { get; }
        public virtual System.Threading.Tasks.Task InitializeAfterCreatingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeAfterShowingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeBeforeCreatingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeBeforeShowingShellAsync() { }
        public virtual System.Threading.Tasks.Task InitializeBeforeShowingSplashScreenAsync() { }
        protected static System.Threading.Tasks.Task RunAndWaitAsync(params System.Func<>[] actions) { }
    }
    public class FlyoutService : Orchestra.Services.IFlyoutService
    {
        public FlyoutService(Catel.IoC.ITypeFactory typeFactory, Catel.MVVM.ICommandManager commandManager) { }
        public void AddFlyout(string name, System.Type viewType, MahApps.Metro.Controls.Position position, Catel.MVVM.Providers.UnloadBehavior unloadBehavior = 1, MahApps.Metro.Controls.FlyoutTheme flyoutTheme = 0) { }
        public System.Collections.Generic.IEnumerable<MahApps.Metro.Controls.Flyout> GetFlyouts() { }
        public void HideFlyout(string name) { }
        public void ShowFlyout(string name, object dataContext) { }
    }
    public class static FlyoutServiceExtensions
    {
        public static void AddFlyout<TView>(this Orchestra.Services.IFlyoutService flyoutService, string name, MahApps.Metro.Controls.Position position, Catel.MVVM.Providers.UnloadBehavior unloadBehavior = 1, MahApps.Metro.Controls.FlyoutTheme flyoutTheme = 0) { }
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
    public interface IFlyoutService
    {
        void AddFlyout(string name, System.Type viewType, MahApps.Metro.Controls.Position position, Catel.MVVM.Providers.UnloadBehavior unloadBehavior = 1, MahApps.Metro.Controls.FlyoutTheme flyoutTheme = 0);
        System.Collections.Generic.IEnumerable<MahApps.Metro.Controls.Flyout> GetFlyouts();
        void HideFlyout(string name);
        void ShowFlyout(string name, object dataContext);
    }
    public interface IMahAppsService : Orchestra.Services.IAboutInfoService, Orchestra.Services.IShellContentService
    {
        MahApps.Metro.Controls.WindowCommands GetRightWindowCommands();
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
        [System.ObsoleteAttribute("App is now constructed with splash by default. Splash can be hidden by using Show" +
            "Splash = false. Will be removed in version 6.0.0.", true)]
        System.Threading.Tasks.Task<TShell> CreateWithSplashAsync<TShell>()
            where TShell :  class, Orchestra.Views.IShell;
    }
    public class MahAppsAboutService : Orchestra.Services.AboutService
    {
        public MahAppsAboutService(Catel.Services.IUIVisualizerService uiVisualizerService, Orchestra.Services.IAboutInfoService aboutInfoService) { }
    }
    public class MahAppsMessageService : Catel.Services.MessageService
    {
        public MahAppsMessageService(Catel.Services.IDispatcherService dispatcherService, Catel.Services.ILanguageService languageService) { }
        protected override System.Threading.Tasks.Task<Catel.Services.MessageResult> ShowMessageBoxAsync(string message, string caption = "", Catel.Services.MessageButton button = 1, Catel.Services.MessageImage icon = 0) { }
    }
    public class MahAppsUIVisualizerService : Catel.Services.UIVisualizerService
    {
        public MahAppsUIVisualizerService(Catel.MVVM.IViewLocator viewLocator) { }
        protected override System.Threading.Tasks.Task<System.Nullable<bool>> ShowWindowAsync(System.Windows.FrameworkElement window, object data, bool showModal) { }
    }
    public class ShellService : Orchestra.Services.IShellService
    {
        public ShellService(Catel.IoC.ITypeFactory typeFactory, Orchestra.Services.IKeyboardMappingsService keyboardMappingsService, Catel.MVVM.ICommandManager commandManager, Orchestra.Services.ISplashScreenService splashScreenService, Orchestra.Services.IEnsureStartupService ensureStartupService, Orchestra.Services.IApplicationInitializationService applicationInitializationService, Catel.IoC.IDependencyResolver dependencyResolver) { }
        public Orchestra.Views.IShell Shell { get; }
        public System.Threading.Tasks.Task<TShell> CreateAsync<TShell>()
            where TShell :  class, Orchestra.Views.IShell { }
        [System.ObsoleteAttribute("App is now constructed with splash by default. Splash can be hidden by using Show" +
            "Splash = false. Will be removed in version 6.0.0.", true)]
        public System.Threading.Tasks.Task<TShell> CreateWithSplashAsync<TShell>()
            where TShell :  class, Orchestra.Views.IShell { }
    }
}
namespace Orchestra.ViewModels
{
    public class ShellViewModel : Catel.MVVM.ViewModelBase
    {
        public ShellViewModel() { }
    }
}
namespace Orchestra.Views
{
    public interface IShell
    {
        void Show();
    }
    public class MahAppsAboutView : Orchestra.Windows.SimpleDataWindow, System.Windows.Markup.IComponentConnector
    {
        public MahAppsAboutView() { }
        public MahAppsAboutView(Orchestra.ViewModels.AboutViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class ShellWindow : Orchestra.Windows.MetroDataWindow, Orchestra.Views.IShell, System.Windows.Markup.IComponentConnector
    {
        public ShellWindow() { }
        public void InitializeComponent() { }
    }
}
namespace Orchestra.Windows
{
    public class MetroDataWindow : MahApps.Metro.Controls.MetroWindow, Catel.MVVM.IViewModelContainer, Catel.MVVM.Views.IDataWindow, Catel.MVVM.Views.IView, System.ComponentModel.INotifyPropertyChanged
    {
        public MetroDataWindow() { }
        public MetroDataWindow(Catel.Windows.DataWindowMode mode, System.Collections.Generic.IEnumerable<Catel.Windows.DataWindowButton> additionalButtons = null, Catel.Windows.DataWindowDefaultButton defaultButton = 0, bool setOwnerAndFocus = True, Catel.Windows.InfoBarMessageControlGenerationMode infoBarMessageControlGenerationMode = 1) { }
        public MetroDataWindow(Catel.MVVM.IViewModel viewModel) { }
        public MetroDataWindow(Catel.MVVM.IViewModel viewModel, Catel.Windows.DataWindowMode mode, System.Collections.Generic.IEnumerable<Catel.Windows.DataWindowButton> additionalButtons = null, Catel.Windows.DataWindowDefaultButton defaultButton = 0, bool setOwnerAndFocus = True, Catel.Windows.InfoBarMessageControlGenerationMode infoBarMessageControlGenerationMode = 1) { }
        protected bool CanClose { get; set; }
        public bool CanCloseUsingEscape { get; set; }
        protected System.Collections.ObjectModel.ReadOnlyCollection<System.Windows.Input.ICommand> Commands { get; }
        protected Catel.Windows.DataWindowDefaultButton DefaultButton { get; }
        protected Catel.Windows.DataWindowMode Mode { get; }
        public Catel.MVVM.IViewModel ViewModel { get; }
        public System.Type ViewModelType { get; }
        public event System.EventHandler<Catel.MVVM.Views.DataContextChangedEventArgs> _viewDataContextChanged;
        public event System.EventHandler<System.EventArgs> _viewLoaded;
        public event System.EventHandler<System.EventArgs> _viewUnloaded;
        public event System.EventHandler<Catel.MVVM.Views.DataContextChangedEventArgs> Catel.MVVM.Views.IView.DataContextChanged;
        public event System.EventHandler<System.EventArgs> Catel.MVVM.Views.IView.Loaded;
        public event System.EventHandler<System.EventArgs> Catel.MVVM.Views.IView.Unloaded;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public event System.EventHandler<System.EventArgs> ViewModelChanged;
        public event System.EventHandler<System.ComponentModel.PropertyChangedEventArgs> ViewModelPropertyChanged;
        protected void AddCustomButton(Catel.Windows.DataWindowButton dataWindowButton) { }
        protected virtual System.Threading.Tasks.Task<bool> ApplyChangesAsync() { }
        protected virtual System.Threading.Tasks.Task<bool> DiscardChangesAsync() { }
        protected System.Threading.Tasks.Task ExecuteApplyAsync() { }
        protected System.Threading.Tasks.Task ExecuteCancelAsync() { }
        protected void ExecuteClose() { }
        protected System.Threading.Tasks.Task ExecuteOkAsync() { }
        protected virtual void Initialize() { }
        protected bool OnApplyCanExecute() { }
        protected System.Threading.Tasks.Task OnApplyExcuteAsync() { }
        protected bool OnCancelCanExecute() { }
        protected System.Threading.Tasks.Task OnCancelExecuteAsync() { }
        protected bool OnCloseCanExecute() { }
        protected void OnCloseExecute() { }
        protected override void OnContentChanged(object oldContent, object newContent) { }
        protected virtual void OnInternalGridChanged() { }
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e) { }
        protected virtual void OnLoaded(System.EventArgs e) { }
        protected bool OnOkCanExecute() { }
        protected System.Threading.Tasks.Task OnOkExecuteAsync() { }
        protected virtual void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e) { }
        protected virtual void OnUnloaded(System.EventArgs e) { }
        protected virtual void OnViewModelChanged() { }
        protected virtual System.Threading.Tasks.Task OnViewModelClosedAsync(object sender, Catel.MVVM.ViewModelClosedEventArgs e) { }
        protected virtual void OnViewModelPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e) { }
        protected void RaiseCanExecuteChangedForAllCommands() { }
        protected virtual bool ValidateData() { }
    }
    public class SimpleDataWindow : MahApps.Metro.Controls.Dialogs.CustomDialog, Catel.MVVM.IViewModelContainer, Catel.MVVM.Views.IDataWindow, Catel.MVVM.Views.IView, System.ComponentModel.INotifyPropertyChanged
    {
        protected SimpleDataWindow() { }
        protected SimpleDataWindow(Catel.Windows.DataWindowMode dataWindowMode) { }
        protected SimpleDataWindow(Catel.MVVM.IViewModel viewModel, Catel.Windows.DataWindowMode mode = 0, System.Collections.Generic.IEnumerable<Catel.Windows.DataWindowButton> additionalButtons = null) { }
        public System.Nullable<bool> DialogResult { get; set; }
        public Catel.MVVM.IViewModel ViewModel { get; }
        public event System.EventHandler<Catel.MVVM.Views.DataContextChangedEventArgs> _viewDataContextChanged;
        public event System.EventHandler<System.EventArgs> _viewLoaded;
        public event System.EventHandler<System.EventArgs> _viewUnloaded;
        public event System.EventHandler<Catel.MVVM.Views.DataContextChangedEventArgs> Catel.MVVM.Views.IView.DataContextChanged;
        public event System.EventHandler<System.EventArgs> Catel.MVVM.Views.IView.Loaded;
        public event System.EventHandler<System.EventArgs> Catel.MVVM.Views.IView.Unloaded;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public event System.EventHandler<System.EventArgs> ViewModelChanged;
        public event System.EventHandler<System.ComponentModel.PropertyChangedEventArgs> ViewModelPropertyChanged;
        protected void AddCustomButton(Catel.Windows.DataWindowButton dataWindowButton) { }
        protected virtual System.Threading.Tasks.Task<bool> ApplyChangesAsync() { }
        public void Close() { }
        protected virtual System.Threading.Tasks.Task<bool> DiscardChangesAsync() { }
        protected System.Threading.Tasks.Task ExecuteApplyAsync() { }
        protected System.Threading.Tasks.Task ExecuteCancelAsync() { }
        protected void ExecuteClose() { }
        protected System.Threading.Tasks.Task ExecuteOkAsync() { }
        protected bool OnApplyCanExecute() { }
        protected System.Threading.Tasks.Task OnApplyExecuteAsync() { }
        public override void OnApplyTemplate() { }
        protected bool OnCancelCanExecute() { }
        protected System.Threading.Tasks.Task OnCancelExecuteAsync() { }
        protected void OnCloseExecute() { }
        protected bool OnOkCanExecute() { }
        protected System.Threading.Tasks.Task OnOkExecuteAsync() { }
        protected virtual bool ValidateData() { }
    }
}