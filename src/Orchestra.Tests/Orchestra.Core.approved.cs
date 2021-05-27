[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v5.0", FrameworkDisplayName="")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Behaviors")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Controls")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Converters")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Markup")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Views")]
[assembly: System.Windows.Markup.XmlnsDefinition("http://schemas.wildgums.com/orchestra", "Orchestra.Windows")]
[assembly: System.Windows.Markup.XmlnsPrefix("http://schemas.wildgums.com/orchestra", "orchestra")]
[assembly: System.Windows.ThemeInfo(System.Windows.ResourceDictionaryLocation.None, System.Windows.ResourceDictionaryLocation.SourceAssembly)]
public static class LoadAssembliesOnStartup { }
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orchestra
{
    public static class ApplicationExtensions
    {
        public static void ApplyTheme(this System.Windows.Application application, bool createStyleForwarders = true) { }
    }
    public abstract class ApplicationWatcherBase
    {
        protected static readonly Catel.Services.IDispatcherService DispatcherService;
        protected ApplicationWatcherBase() { }
        protected void EnqueueShellActivatedAction(System.Action<System.Windows.Window> action) { }
    }
    public static class AssemblyExtensions
    {
        public static System.Drawing.Icon ExtractAssemblyIcon(this System.Reflection.Assembly assembly) { }
        public static System.Windows.Media.Imaging.BitmapImage ExtractLargestIcon(this System.Reflection.Assembly assembly) { }
    }
    public abstract class CloseApplicationWatcherBase : Orchestra.ApplicationWatcherBase
    {
        protected CloseApplicationWatcherBase() { }
        protected virtual System.Threading.Tasks.Task<bool> ClosingAsync() { }
        protected virtual void ClosingCanceled() { }
        protected virtual void ClosingFailed(Orchestra.ClosingDetails appClosingFaultDetails) { }
        protected virtual System.Threading.Tasks.Task<bool> PrepareClosingAsync() { }
    }
    public class ClosingDetails
    {
        public ClosingDetails() { }
        public bool CanBeClosed { get; set; }
        public bool CanKeepOpened { get; set; }
        public System.Exception Exception { get; set; }
        public string Message { get; set; }
        public System.Windows.Window Window { get; set; }
    }
    public static class DependencyObjectExtensions
    {
        public static T Clone<T>(this T source)
            where T : System.Windows.DependencyObject { }
        public static System.Windows.Window GetParentWindow(this System.Windows.DependencyObject visualObject) { }
    }
    public static class DotNetPatchHelper
    {
        public static void Attach() { }
        public static void Detach() { }
        public static void Initialize() { }
    }
    public class FileBasedThirdPartyNotice : Orchestra.ThirdPartyNotice
    {
        public FileBasedThirdPartyNotice(string title, string url, string fileName) { }
    }
    public static class FilterHelper
    {
        public static bool MatchesFilters(System.Collections.Generic.IEnumerable<string> filters, string fileName) { }
    }
    public class FontThirdPartyNotice : Orchestra.ThirdPartyNotice
    {
        public FontThirdPartyNotice(string fontName, string fontUrl) { }
    }
    public static class FrameworkElementExtensions { }
    public static class IconExtensions
    {
        public static System.Windows.Media.ImageSource ToImageSource(this System.Drawing.Icon icon, int requiredSize = 64) { }
    }
    public class KeyPressApplicationWatcherBase : Orchestra.ApplicationWatcherBase
    {
        public KeyPressApplicationWatcherBase() { }
        protected virtual void OnKeyDown(System.Windows.Input.KeyEventArgs e) { }
        protected virtual void OnKeyUp(System.Windows.Input.KeyEventArgs e) { }
        protected virtual void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e) { }
        protected virtual void OnPreviewKeyUp(System.Windows.Input.KeyEventArgs e) { }
    }
    public class KeyPressWindowWatcher
    {
        public KeyPressWindowWatcher() { }
        public void SetKeyDownHandler(System.Action<System.Windows.Input.KeyEventArgs> handler) { }
        public void SetKeyUpHandler(System.Action<System.Windows.Input.KeyEventArgs> handler) { }
        public void SetPreviewKeyDownHandler(System.Action<System.Windows.Input.KeyEventArgs> handler) { }
        public void SetPreviewKeyUpHandler(System.Action<System.Windows.Input.KeyEventArgs> handler) { }
        public void UnWatchWindow(System.Windows.Window window) { }
        public void WatchWindow(System.Windows.Window window) { }
        public static bool IsAltHeldDown() { }
        public static bool IsCtrlHeldDown() { }
        public static bool IsKeyHeldDown(System.Windows.Input.Key key) { }
        public static bool IsShiftHeldDown() { }
    }
    public static class LogFilePrefixes
    {
        public static readonly string[] All;
        public static readonly string CrashReport;
        public static readonly string EntryAssemblyName;
        public static readonly string Log;
    }
    public static class LogHelper
    {
        public static void AddFileLogListener() { }
        public static System.Threading.Tasks.Task AddLogListenerForUnhandledExceptionAsync(System.Exception ex) { }
        public static void CleanUpAllLogTypeFiles(bool keepCleanInRealTime = false) { }
        public static Catel.Logging.ILogListener CreateFileLogListener(string prefix) { }
    }
    public static class OrchestraEnvironment
    {
        public static readonly System.Windows.Media.SolidColorBrush DefaultAccentColorBrush;
    }
    public class OrchestraException : System.Exception
    {
        public OrchestraException(string message) { }
        public OrchestraException(string message, System.Exception innerException) { }
    }
    public class ResourceBasedThirdPartyNotice : Orchestra.ThirdPartyNotice
    {
        public ResourceBasedThirdPartyNotice(string title, string url, System.Reflection.Assembly assembly, string relativeResourceName) { }
        public ResourceBasedThirdPartyNotice(string title, string url, string assemblyName, string relativeResourceName) { }
        public ResourceBasedThirdPartyNotice(string title, string url, System.Reflection.Assembly assembly, string rootNamespace, string relativeResourceName) { }
        public ResourceBasedThirdPartyNotice(string title, string url, string assemblyName, string rootNamespace, string relativeResourceName) { }
    }
    public static class StringExtensions
    {
        public static string GetCommandGroup(this string commandName) { }
        public static string GetCommandName(this string commandName) { }
    }
    public class ThirdPartyNotice
    {
        public ThirdPartyNotice() { }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
    public abstract class ToggleConfigurationCommandContainerBase : Orchestra.ToggleConfigurationCommandContainerBase<object>
    {
        protected ToggleConfigurationCommandContainerBase(string commandName, string configurationKey, bool defaultValue, Catel.MVVM.ICommandManager commandManager, Catel.Configuration.IConfigurationService configurationService) { }
    }
    public abstract class ToggleConfigurationCommandContainerBase<TParameter> : Orchestra.ToggleConfigurationCommandContainerBase<TParameter, TParameter>
    {
        protected ToggleConfigurationCommandContainerBase(string commandName, string configurationKey, bool defaultValue, Catel.MVVM.ICommandManager commandManager, Catel.Configuration.IConfigurationService configurationService) { }
    }
    public abstract class ToggleConfigurationCommandContainerBase<TExecuteParameter, TCanExecuteParameter> : Catel.MVVM.CommandContainerBase<TExecuteParameter, TCanExecuteParameter>
    {
        protected ToggleConfigurationCommandContainerBase(string commandName, string configurationKey, bool defaultValue, Catel.MVVM.ICommandManager commandManager, Catel.Configuration.IConfigurationService configurationService) { }
        protected Catel.Configuration.IConfigurationService ConfigurationService { get; }
        protected override System.Threading.Tasks.Task ExecuteAsync(TExecuteParameter parameter) { }
    }
    public static class VersionHelper
    {
        public static string GetCurrentVersion(System.Reflection.Assembly assembly = null) { }
    }
    public static class WindowExtensions
    {
        public static void BringWindowToTop(this System.Windows.FrameworkElement frameworkElement) { }
        public static void CenterWindowToScreen(this System.Windows.Window window) { }
        public static void DisableCloseButton(this System.Windows.Window window) { }
        public static void SetMaximumHeight(this System.Windows.Window window) { }
        public static void SetMaximumWidth(this System.Windows.Window window) { }
        public static void SetMaximumWidthAndHeight(this System.Windows.Window window) { }
    }
}
namespace Orchestra.Behaviors
{
    public class HintsBehavior : Catel.Windows.Interactivity.BehaviorBase<System.Windows.FrameworkElement>
    {
        public static readonly System.Windows.DependencyProperty AdornerProperty;
        public HintsBehavior(Orchestra.Services.IAdorneredTooltipsManagerFactory adorneredTooltipsManagerFactory) { }
        public System.Windows.Media.Visual Adorner { get; set; }
        protected override void Initialize() { }
    }
    public class RememberWindowSize : Catel.Windows.Interactivity.BehaviorBase<System.Windows.Window>
    {
        public static readonly System.Windows.DependencyProperty MakeWindowResizableProperty;
        public static readonly System.Windows.DependencyProperty RememberWindowStateProperty;
        public RememberWindowSize() { }
        public bool MakeWindowResizable { get; set; }
        public bool RememberWindowState { get; set; }
        protected override void OnAssociatedObjectLoaded() { }
        protected override void OnAssociatedObjectUnloaded() { }
    }
}
namespace Orchestra.Changelog
{
    public class Changelog
    {
        public Changelog() { }
        public bool IsEmpty { get; }
        public System.Collections.Generic.List<Orchestra.Changelog.ChangelogItem> Items { get; }
        public string Title { get; set; }
    }
    public static class ChangelogExtensions
    {
        public static Orchestra.Changelog.Changelog GetDelta(this Orchestra.Changelog.Changelog changelog1, Orchestra.Changelog.Changelog changelog2) { }
    }
    public class ChangelogItem
    {
        public ChangelogItem() { }
        public string Description { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public object Tag { get; set; }
        public Orchestra.Changelog.ChangelogType Type { get; set; }
        public override string ToString() { }
    }
    public abstract class ChangelogProviderBase : Orchestra.Changelog.IChangelogProvider
    {
        protected ChangelogProviderBase() { }
        public abstract System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Orchestra.Changelog.ChangelogItem>> GetChangelogAsync();
    }
    public class ChangelogService : Orchestra.Changelog.IChangelogService
    {
        public ChangelogService(Catel.IoC.ITypeFactory typeFactory, Orchestra.Changelog.IChangelogSnapshotService changelogSnapshotService) { }
        public virtual System.Threading.Tasks.Task<Orchestra.Changelog.Changelog> GetChangelogAsync() { }
        protected virtual System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Orchestra.Changelog.ChangelogItem>> GetChangelogAsync(Orchestra.Changelog.IChangelogProvider provider) { }
        public virtual System.Threading.Tasks.Task<Orchestra.Changelog.Changelog> GetChangelogSinceSnapshotAsync() { }
    }
    public class ChangelogSnapshotService : Orchestra.Changelog.IChangelogSnapshotService
    {
        public ChangelogSnapshotService(Orc.FileSystem.IDirectoryService directoryService, Orc.FileSystem.IFileService fileService, Catel.Services.IAppDataService appDataService) { }
        public virtual System.Threading.Tasks.Task<Orchestra.Changelog.Changelog> DeserializeSnapshotAsync() { }
        protected virtual string GetFilename() { }
        protected virtual Newtonsoft.Json.JsonSerializerSettings GetSerializerSettings() { }
        public virtual System.Threading.Tasks.Task SerializeSnapshotAsync(Orchestra.Changelog.Changelog changelog) { }
    }
    public enum ChangelogType
    {
        Change = 0,
        Improvement = 1,
        Feature = 2,
        Bug = 3,
    }
    [System.Windows.Markup.MarkupExtensionReturnType(typeof(object))]
    public class ChangelogTypeIconExtension : Catel.Windows.Markup.UpdatableMarkupExtension
    {
        public ChangelogTypeIconExtension() { }
        public Orchestra.Changelog.ChangelogType? ChangelogType { get; set; }
        public System.Windows.Data.BindingBase ChangelogTypeBinding { get; set; }
        protected override void OnTargetObjectLoaded() { }
        protected override object ProvideDynamicValue(System.IServiceProvider serviceProvider) { }
    }
    public interface IChangelogProvider
    {
        System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Orchestra.Changelog.ChangelogItem>> GetChangelogAsync();
    }
    public interface IChangelogService
    {
        System.Threading.Tasks.Task<Orchestra.Changelog.Changelog> GetChangelogAsync();
        System.Threading.Tasks.Task<Orchestra.Changelog.Changelog> GetChangelogSinceSnapshotAsync();
    }
    public static class IChangelogServiceExtensions
    {
        public static System.Threading.Tasks.Task<System.Collections.Generic.List<Orchestra.Changelog.ChangelogItem>> GetChangelogItemsForGroupAsync(this Orchestra.Changelog.IChangelogService changelogService, string groupName) { }
    }
    public interface IChangelogSnapshotService
    {
        System.Threading.Tasks.Task<Orchestra.Changelog.Changelog> DeserializeSnapshotAsync();
        System.Threading.Tasks.Task SerializeSnapshotAsync(Orchestra.Changelog.Changelog snapshot);
    }
}
namespace Orchestra.Changelog.ViewModels
{
    public class ChangelogViewModel : Catel.MVVM.ViewModelBase
    {
        public ChangelogViewModel(Orchestra.Changelog.Changelog changelog, Orchestra.Changelog.IChangelogService changelogService, Orchestra.Changelog.IChangelogSnapshotService changelogSnapshotService) { }
        public Orchestra.Changelog.Changelog Changelog { get; }
        public System.Collections.Generic.List<Orchestra.Changelog.ChangelogItem> Items { get; }
        protected override System.Threading.Tasks.Task<bool> SaveAsync() { }
    }
}
namespace Orchestra.Changelog.Views
{
    public class ChangelogView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public ChangelogView() { }
        public void InitializeComponent() { }
    }
    public class ChangelogWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public ChangelogWindow() { }
        public void InitializeComponent() { }
    }
}
namespace Orchestra.Collections
{
    public class AdorneredTooltipsCollection : Orchestra.Collections.IAdorneredTooltipsCollection
    {
        public AdorneredTooltipsCollection(Orchestra.Tooltips.IAdorneredTooltipFactory factory) { }
        public void Add(System.Windows.FrameworkElement parentControl, System.Windows.Documents.Adorner tooltip, bool adornerLayerVisibility) { }
        public void AdornerLayerDisabled() { }
        public void AdornerLayerEnabled() { }
        public System.Collections.Generic.IList<Orchestra.Tooltips.IAdorneredTooltip> GetAdorners(System.Windows.FrameworkElement parentControl) { }
        public void HideAll() { }
        public void Remove(System.Windows.FrameworkElement parentControl) { }
        public void ShowAll() { }
    }
    public interface IAdorneredTooltipsCollection
    {
        void Add(System.Windows.FrameworkElement parentControl, System.Windows.Documents.Adorner tooltip, bool adornerLayerVisibility);
        void AdornerLayerDisabled();
        void AdornerLayerEnabled();
        System.Collections.Generic.IList<Orchestra.Tooltips.IAdorneredTooltip> GetAdorners(System.Windows.FrameworkElement parentControl);
        void HideAll();
        void Remove(System.Windows.FrameworkElement parentControl);
        void ShowAll();
    }
}
namespace Orchestra.Configuration
{
    public static class ConfigurationExtensions
    {
        public static bool IsConfigurationKey(this Catel.Configuration.ConfigurationChangedEventArgs e, string expectedKey) { }
        public static bool IsConfigurationKey(this string key, string expectedKey) { }
    }
    public abstract class ConfigurationSynchronizerBase<T> : Catel.IoC.INeedCustomInitialization
    {
        protected ConfigurationSynchronizerBase(string key, T defaultValue, Catel.Configuration.IConfigurationService configurationService) { }
        protected ConfigurationSynchronizerBase(string key, T defaultValue, Catel.Configuration.ConfigurationContainer container, Catel.Configuration.IConfigurationService configurationService) { }
        protected bool ApplyAtStartup { get; set; }
        protected Catel.Configuration.IConfigurationService ConfigurationService { get; }
        protected Catel.Configuration.ConfigurationContainer Container { get; }
        protected T DefaultValue { get; }
        protected string Key { get; }
        public virtual void ApplyConfiguration() { }
        protected virtual void ApplyConfiguration(T value) { }
        protected virtual System.Threading.Tasks.Task ApplyConfigurationAsync(T value) { }
        public virtual T GetCurrentValue() { }
        protected abstract string GetStatus(T value);
    }
}
namespace Orchestra.Controls
{
    public class KeyboardMappingControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public KeyboardMappingControl() { }
        public void InitializeComponent() { }
    }
}
namespace Orchestra.Converters
{
    public class BooleanToThicknessConverter : Catel.MVVM.Converters.ValueConverterBase<bool>
    {
        public BooleanToThicknessConverter() { }
        protected override object Convert(bool value, System.Type targetType, object parameter) { }
    }
    public class CommandNameToStringConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public CommandNameToStringConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class InputGestureToStringConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public InputGestureToStringConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class KeyboardMappingToStringConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public KeyboardMappingToStringConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class NullImageSourceConverter : Catel.MVVM.Converters.ValueConverterBase
    {
        public NullImageSourceConverter() { }
        protected override object Convert(object value, System.Type targetType, object parameter) { }
    }
    public class PathToStringConverter : Catel.MVVM.Converters.ValueConverterBase<string>
    {
        public PathToStringConverter() { }
        protected override object Convert(string value, System.Type targetType, object parameter) { }
    }
}
namespace Orchestra.Layers
{
    public class HintsAdornerLayer : Orchestra.Layers.IAdornerLayer
    {
        public HintsAdornerLayer(System.Windows.Documents.AdornerLayer adornerLayer) { }
        public void Add(System.Windows.Documents.Adorner adorner) { }
        public System.Windows.Documents.Adorner[] GetAdorners(System.Windows.UIElement adornedElement) { }
    }
    public interface IAdornerLayer
    {
        void Add(System.Windows.Documents.Adorner adorner);
        System.Windows.Documents.Adorner[] GetAdorners(System.Windows.UIElement adornedElement);
    }
}
namespace Orchestra.Logging
{
    public class FileLogListener : Catel.Logging.FileLogListener
    {
        public FileLogListener(System.Reflection.Assembly assembly = null) { }
        public FileLogListener(string filePath, int maxSizeInKiloBytes, System.Reflection.Assembly assembly = null) { }
        protected override bool ShouldIgnoreLogMessage(Catel.Logging.ILog log, string message, Catel.Logging.LogEvent logEvent, object extraData, Catel.Logging.LogData logData, System.DateTime time) { }
    }
    public class RichTextBoxLogListener : Catel.Logging.LogListenerBase
    {
        public RichTextBoxLogListener(System.Windows.Controls.RichTextBox richTextBox) { }
        public void Clear() { }
        protected override void Write(Catel.Logging.ILog log, string message, Catel.Logging.LogEvent logEvent, object extraData, Catel.Logging.LogData logData, System.DateTime time) { }
    }
    public class StatusLogListener : Catel.Logging.StatusLogListener
    {
        public StatusLogListener(Orchestra.Services.IStatusService statusService) { }
        protected override void Write(Catel.Logging.ILog log, string message, Catel.Logging.LogEvent logEvent, object extraData, Catel.Logging.LogData logData, System.DateTime time) { }
    }
    public class TextBoxLogListener : Catel.Logging.LogListenerBase
    {
        public TextBoxLogListener(System.Windows.Controls.TextBox textBox) { }
        public void Clear() { }
        protected override void Write(Catel.Logging.ILog log, string message, Catel.Logging.LogEvent logEvent, object extraData, Catel.Logging.LogData logData, System.DateTime time) { }
    }
}
namespace Orchestra.Markup
{
    public class CanvasViewbox : Catel.Windows.Markup.UpdatableMarkupExtension
    {
        public CanvasViewbox() { }
        public CanvasViewbox(string pathName) { }
        public System.Windows.Media.SolidColorBrush Foreground { get; set; }
        [System.Windows.Markup.ConstructorArgument("pathName")]
        public string PathName { get; set; }
        protected override object ProvideDynamicValue(System.IServiceProvider serviceProvider) { }
    }
}
namespace Orchestra.Models
{
    public class AboutInfo : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData AppIconProperty;
        public static readonly Catel.Data.PropertyData AssemblyProperty;
        public static readonly Catel.Data.PropertyData BuildDateTimeProperty;
        public static readonly Catel.Data.PropertyData CompanyLogoForSplashScreenUriProperty;
        public static readonly Catel.Data.PropertyData CompanyLogoUriProperty;
        public static readonly Catel.Data.PropertyData CompanyProperty;
        public static readonly Catel.Data.PropertyData CopyrightProperty;
        public static readonly Catel.Data.PropertyData CopyrightUriProperty;
        public static readonly Catel.Data.PropertyData DescriptionProperty;
        public static readonly Catel.Data.PropertyData DisplayVersionProperty;
        public static readonly Catel.Data.PropertyData InformationalVersionProperty;
        public static readonly Catel.Data.PropertyData LogoImageSourceProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public static readonly Catel.Data.PropertyData ProductNameProperty;
        public static readonly Catel.Data.PropertyData ShowLogButtonProperty;
        public static readonly Catel.Data.PropertyData UriInfoProperty;
        public static readonly Catel.Data.PropertyData VersionProperty;
        public AboutInfo(
                    System.Uri companyLogoUri = null,
                    string logoImageSource = null,
                    Orchestra.Models.UriInfo uriInfo = null,
                    System.Reflection.Assembly assembly = null,
                    System.Uri companyLogoForSplashScreenUri = null,
                    System.Windows.Media.Imaging.BitmapSource appIcon = null,
                    System.DateTime? buildDateTime = default,
                    string company = null,
                    string copyright = null,
                    System.Uri copyrightUri = null,
                    string description = null,
                    string displayVersion = null,
                    string informationalVersion = null,
                    string name = null,
                    string productName = null,
                    string version = null) { }
        public System.Windows.Media.Imaging.BitmapSource AppIcon { get; }
        public System.Reflection.Assembly Assembly { get; }
        public System.DateTime? BuildDateTime { get; }
        public string Company { get; }
        public System.Uri CompanyLogoForSplashScreenUri { get; set; }
        public System.Uri CompanyLogoUri { get; }
        public string Copyright { get; }
        public System.Uri CopyrightUri { get; }
        public string Description { get; }
        public string DisplayVersion { get; }
        public string InformationalVersion { get; }
        public string LogoImageSource { get; }
        public string Name { get; }
        public string ProductName { get; }
        public bool ShowLogButton { get; set; }
        public Orchestra.Models.UriInfo UriInfo { get; }
        public string Version { get; }
    }
    public class CommandInfo : Orchestra.Models.ICommandInfo
    {
        public CommandInfo(string commandName, Catel.Windows.Input.InputGesture inputGesture) { }
        public string CommandName { get; }
        public Catel.Windows.Input.InputGesture InputGesture { get; set; }
        public bool IsHidden { get; set; }
    }
    public class Hint : Orchestra.Models.IHint
    {
        public Hint(string text, string controlName) { }
        public string ControlName { get; }
        public string Text { get; }
    }
    public interface ICommandInfo
    {
        string CommandName { get; }
        Catel.Windows.Input.InputGesture InputGesture { get; set; }
        bool IsHidden { get; set; }
    }
    public interface IHint
    {
        string ControlName { get; }
        string Text { get; }
    }
    public class KeyboardMapping : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData CommandNameProperty;
        public static readonly Catel.Data.PropertyData InputGestureProperty;
        public static readonly Catel.Data.PropertyData IsEditableProperty;
        public static readonly Catel.Data.PropertyData TextProperty;
        public KeyboardMapping() { }
        public KeyboardMapping(string commandName, string additionalText, System.Windows.Input.ModifierKeys modifierKeys) { }
        public string CommandName { get; set; }
        public Catel.Windows.Input.InputGesture InputGesture { get; set; }
        public bool IsEditable { get; set; }
        public string Text { get; set; }
    }
    public class KeyboardMappings : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData GroupNameProperty;
        public static readonly Catel.Data.PropertyData MappingsProperty;
        public KeyboardMappings() { }
        public string GroupName { get; set; }
        public System.Collections.Generic.List<Orchestra.Models.KeyboardMapping> Mappings { get; }
    }
    public class RecentlyUsedItem : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData DateTimeProperty;
        public static readonly Catel.Data.PropertyData NameProperty;
        public RecentlyUsedItem() { }
        public RecentlyUsedItem(string name, System.DateTime dateTime) { }
        public System.DateTime DateTime { get; }
        public string Name { get; }
    }
    public class RecentlyUsedItems : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData ItemsProperty;
        public static readonly Catel.Data.PropertyData PinnedItemsProperty;
        public RecentlyUsedItems() { }
        public System.Collections.Generic.List<Orchestra.Models.RecentlyUsedItem> Items { get; }
        public System.Collections.Generic.List<Orchestra.Models.RecentlyUsedItem> PinnedItems { get; }
    }
    public class UriInfo
    {
        public UriInfo(string uri, string displayText = null) { }
        public string DisplayText { get; set; }
        public string Uri { get; set; }
    }
}
namespace Orchestra.Services
{
    public class AboutInfoService : Orchestra.Services.IAboutInfoService
    {
        public AboutInfoService() { }
        public Orchestra.Models.AboutInfo GetAboutInfo() { }
    }
    public class AboutService : Orchestra.Services.IAboutService
    {
        public AboutService(Catel.Services.IUIVisualizerService uiVisualizerService, Orchestra.Services.IAboutInfoService aboutInfoService) { }
        public virtual System.Threading.Tasks.Task ShowAboutAsync() { }
    }
    public class AdorneredTooltipsManager : Orchestra.Services.IAdorneredTooltipsManager
    {
        public AdorneredTooltipsManager(Orchestra.Services.IAdornerTooltipGenerator adornerTooltipGenerator, Orchestra.Services.IHintsProvider hintsProviderProvider, Orchestra.Layers.IAdornerLayer adornerLayer, Orchestra.Collections.IAdorneredTooltipsCollection adorneredTooltipsCollection) { }
        public bool IsEnabled { get; }
        public void AddHintsFor(System.Windows.FrameworkElement element) { }
        public void Disable() { }
        public void Enable() { }
        public void HideHints() { }
        public void ShowHints() { }
    }
    public class AdorneredTooltipsManagerFactory : Orchestra.Services.IAdorneredTooltipsManagerFactory
    {
        public AdorneredTooltipsManagerFactory(Catel.IoC.IServiceLocator serviceLocator, Catel.IoC.ITypeFactory typeFactory) { }
        public Orchestra.Services.IAdorneredTooltipsManager Create(System.Windows.Documents.AdornerLayer adornerLayer) { }
    }
    public class ClipboardService : Orchestra.Services.IClipboardService
    {
        public ClipboardService() { }
        public void CopyToClipboard(string text) { }
    }
    public class CloseApplicationService : Orchestra.Services.ICloseApplicationService
    {
        public CloseApplicationService(Orchestra.Services.IEnsureStartupService ensureStartupService) { }
        public void Close() { }
        public System.Threading.Tasks.Task CloseAsync() { }
    }
    public class CommandInfoService : Orchestra.Services.ICommandInfoService
    {
        public CommandInfoService(Catel.MVVM.ICommandManager commandManager) { }
        public Orchestra.Models.ICommandInfo GetCommandInfo(string commandName) { }
        public void Invalidate() { }
        public void UpdateCommandInfo(string commandName, Orchestra.Models.ICommandInfo commandInfo) { }
    }
    public class EnsureStartupService : Orchestra.Services.IEnsureStartupService
    {
        public EnsureStartupService(Catel.Services.IAppDataService appDataService, Catel.Services.IUIVisualizerService uiVisualizerService, Orc.FileSystem.IFileService fileService) { }
        public bool SuccessfullyStarted { get; }
        public virtual void ConfirmApplicationStartedSuccessfully() { }
        public virtual System.Threading.Tasks.Task EnsureFailSafeStartupAsync() { }
        protected bool IsFileLocked(string fileName) { }
    }
    public class HintsProvider : Orchestra.Services.IHintsProvider
    {
        public HintsProvider() { }
        public void AddHint<TControlType>(Orchestra.Models.IHint hint) { }
        public void AddHint<TControlType>(string hintText, System.Linq.Expressions.Expression<System.Func<object>> userControlName) { }
        public Orchestra.Models.IHint[] GetHintsFor(System.Windows.FrameworkElement element) { }
    }
    public interface IAboutInfoService
    {
        Orchestra.Models.AboutInfo GetAboutInfo();
    }
    public interface IAboutService
    {
        System.Threading.Tasks.Task ShowAboutAsync();
    }
    public interface IAdornerTooltipGenerator
    {
        System.Windows.Documents.Adorner GetAdornerTooltip(Orchestra.Models.IHint hint, System.Windows.UIElement adornedElement);
    }
    public interface IAdorneredTooltipsManager
    {
        bool IsEnabled { get; }
        void AddHintsFor(System.Windows.FrameworkElement element);
        void Disable();
        void Enable();
        void HideHints();
        void ShowHints();
    }
    public interface IAdorneredTooltipsManagerFactory
    {
        Orchestra.Services.IAdorneredTooltipsManager Create(System.Windows.Documents.AdornerLayer adornerLayer);
    }
    public interface IClipboardService
    {
        void CopyToClipboard(string text);
    }
    public interface ICloseApplicationService
    {
        System.Threading.Tasks.Task CloseAsync();
    }
    public interface ICommandInfoService
    {
        Orchestra.Models.ICommandInfo GetCommandInfo(string commandName);
        void Invalidate();
        void UpdateCommandInfo(string commandName, Orchestra.Models.ICommandInfo commandInfo);
    }
    public static class ICommandInfoServiceExtensions
    {
        public static void UpdateCommandInfo(this Orchestra.Services.ICommandInfoService commandInfoService, string commandName, System.Action<Orchestra.Models.ICommandInfo> commandInfoUpdateCallback) { }
    }
    public interface IEnsureStartupService
    {
        bool SuccessfullyStarted { get; }
        void ConfirmApplicationStartedSuccessfully();
        System.Threading.Tasks.Task EnsureFailSafeStartupAsync();
    }
    public interface IHintsProvider
    {
        Orchestra.Models.IHint[] GetHintsFor(System.Windows.FrameworkElement element);
    }
    public interface IKeyboardMappingsAllowedKeysService
    {
        bool IsAllowed(System.Windows.Input.Key key);
    }
    public interface IKeyboardMappingsService
    {
        System.Collections.Generic.List<Orchestra.Models.KeyboardMapping> AdditionalKeyboardMappings { get; }
        void Load();
        void Reset();
        void Save();
    }
    public interface IManageAppDataService
    {
        System.Collections.Generic.List<string> ExclusionFilters { get; }
        System.Threading.Tasks.Task<bool> BackupUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget);
        System.Threading.Tasks.Task DeleteUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget);
        bool OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget applicationDataTarget);
    }
    public static class IMessageServiceExtensions
    {
        public static string GetAsText(this Catel.Services.IMessageService messageService, string message, Catel.Services.MessageButton messageButton) { }
        public static string GetAsText(this Catel.Services.IMessageService messageService, string message, string buttons) { }
    }
    public interface IRecentlyUsedItemsService
    {
        System.Collections.Generic.IEnumerable<Orchestra.Models.RecentlyUsedItem> Items { get; }
        int MaximumItemCount { get; set; }
        System.Collections.Generic.IEnumerable<Orchestra.Models.RecentlyUsedItem> PinnedItems { get; }
        event System.EventHandler<System.EventArgs> Updated;
        void AddItem(Orchestra.Models.RecentlyUsedItem item);
        void PinItem(string name);
        void RemoveItem(Orchestra.Models.RecentlyUsedItem item);
        void UnpinItem(string name);
    }
    public interface ISplashScreenService
    {
        System.Windows.Window CreateSplashScreen();
    }
    public interface IStatusFilterService
    {
        bool IsSuspended { get; set; }
        string GetStatus(string status);
    }
    public interface IStatusService
    {
        void Initialize(Orc.Controls.Services.IStatusRepresenter statusRepresenter);
        void UpdateStatus(string status);
    }
    public static class IStatusServiceExtensions
    {
        public static void UpdateStatus(this Orchestra.Services.IStatusService statusService, string statusFormat, params object[] parameters) { }
    }
    public interface IThirdPartyNoticesService
    {
        void Add(Orchestra.ThirdPartyNotice thirdPartyNotice);
        System.Collections.Generic.List<Orchestra.ThirdPartyNotice> GetThirdPartyNotices();
    }
    public static class IThirdPartyNoticesServiceExtensions
    {
        public static void AddWithTryCatch(this Orchestra.Services.IThirdPartyNoticesService thirdPartyNoticesService, System.Func<Orchestra.ThirdPartyNotice> func) { }
    }
    public interface IViewActivationService
    {
        bool Activate(Catel.MVVM.IViewModel viewModel);
        bool Activate(System.Type viewModelType);
    }
    public static class IViewActivationServiceExtensions
    {
        public static System.Threading.Tasks.Task ActivateOrShowAsync(this Orchestra.Services.IViewActivationService viewActivationService, Catel.MVVM.IViewModel viewModel) { }
        public static System.Threading.Tasks.Task ActivateOrShowAsync(this Orchestra.Services.IViewActivationService viewActivationService, System.Type viewModelType) { }
        public static System.Threading.Tasks.Task ActivateOrShowAsync<TViewModel>(this Orchestra.Services.IViewActivationService viewActivationService)
            where TViewModel : Catel.MVVM.IViewModel { }
    }
    public interface IXamlResourceService
    {
        System.Collections.Generic.IEnumerable<System.Windows.ResourceDictionary> GetApplicationResourceDictionaries();
    }
    public class KeyboardMappingsAllowedKeysService : Orchestra.Services.IKeyboardMappingsAllowedKeysService
    {
        protected readonly System.Collections.Generic.HashSet<System.Windows.Input.Key> IgnoredKeys;
        public KeyboardMappingsAllowedKeysService() { }
        public virtual bool IsAllowed(System.Windows.Input.Key key) { }
    }
    public class KeyboardMappingsService : Orchestra.Services.IKeyboardMappingsService
    {
        public KeyboardMappingsService(Catel.MVVM.ICommandManager commandManager, Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer, Orc.FileSystem.IFileService fileService, Catel.Services.IAppDataService appDataService) { }
        public System.Collections.Generic.List<Orchestra.Models.KeyboardMapping> AdditionalKeyboardMappings { get; }
        public void Load() { }
        public void Reset() { }
        public void Save() { }
    }
    public class ManageAppDataService : Orchestra.Services.IManageAppDataService
    {
        public ManageAppDataService(Catel.Services.ISaveFileService saveFileService, Catel.Services.IProcessService processService, Orc.FileSystem.IDirectoryService directoryService, Orc.FileSystem.IFileService fileService, Catel.Services.IAppDataService appDataService) { }
        public System.Collections.Generic.List<string> ExclusionFilters { get; }
        public System.Threading.Tasks.Task<bool> BackupUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget) { }
        public System.Threading.Tasks.Task DeleteUserDataAsync(Catel.IO.ApplicationDataTarget applicationDataTarget) { }
        protected virtual bool MatchesFilters(System.Collections.Generic.IEnumerable<string> filters, string fileName) { }
        public bool OpenApplicationDataDirectory(Catel.IO.ApplicationDataTarget applicationDataTarget) { }
    }
    public class MessageService : Catel.Services.MessageService
    {
        public MessageService(Catel.Services.IDispatcherService dispatcherService, Catel.Services.IUIVisualizerService uiVisualizerService, Catel.MVVM.IViewModelFactory viewModelFactory, Catel.Services.ILanguageService languageService) { }
        public override System.Threading.Tasks.Task<Catel.Services.MessageResult> ShowAsync(string message, string caption = "", Catel.Services.MessageButton button = 1, Catel.Services.MessageImage icon = 0) { }
    }
    public class MicrosoftApiSelectDirectoryService : Catel.Services.ISelectDirectoryService
    {
        public MicrosoftApiSelectDirectoryService() { }
        public string DirectoryName { get; }
        public string FileName { get; set; }
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }
        public bool ShowNewFolderButton { get; set; }
        public string Title { get; set; }
        public System.Threading.Tasks.Task<bool> DetermineDirectoryAsync() { }
        public System.Threading.Tasks.Task<Catel.Services.DetermineDirectoryResult> DetermineDirectoryAsync(Catel.Services.DetermineDirectoryContext context) { }
    }
    public class PleaseWaitService : Catel.Services.IPleaseWaitService
    {
        protected readonly Catel.Services.IDispatcherService _dispatcherService;
        public PleaseWaitService(Catel.Services.IDispatcherService dispatcherService) { }
        protected int CurrentItem { get; }
        protected bool ReachedTotalItems { get; }
        public int ShowCounter { get; }
        protected int TotalItems { get; }
        public virtual void Hide() { }
        protected virtual void HideIfRequired() { }
        public virtual void Pop() { }
        public virtual void Push(string status = "") { }
        public virtual void Show(string status = "") { }
        public virtual void Show(Catel.Services.PleaseWaitWorkDelegate workDelegate, string status = "") { }
        public virtual void UpdateStatus(string status) { }
        public virtual void UpdateStatus(int currentItem, int totalItems, string statusFormat = "") { }
    }
    public class RecentlyUsedItemsService : Orchestra.Services.IRecentlyUsedItemsService
    {
        public RecentlyUsedItemsService(Catel.Runtime.Serialization.Xml.IXmlSerializer xmlSerializer, Orc.FileSystem.IFileService fileService, Catel.Services.IAppDataService appDataService) { }
        public System.Collections.Generic.IEnumerable<Orchestra.Models.RecentlyUsedItem> Items { get; }
        public int MaximumItemCount { get; set; }
        public System.Collections.Generic.IEnumerable<Orchestra.Models.RecentlyUsedItem> PinnedItems { get; }
        public event System.EventHandler<System.EventArgs> Updated;
        public void AddItem(Orchestra.Models.RecentlyUsedItem item) { }
        public void PinItem(string name) { }
        public void RemoveItem(Orchestra.Models.RecentlyUsedItem item) { }
        public void UnpinItem(string name) { }
    }
    public class SplashScreenService : Orchestra.Services.ISplashScreenService
    {
        public SplashScreenService() { }
        public System.Windows.Window CreateSplashScreen() { }
    }
    public class StatusFilterService : Orchestra.Services.IStatusFilterService
    {
        public StatusFilterService() { }
        public bool IsSuspended { get; set; }
        public string GetStatus(string status) { }
    }
    public class StatusService : Orchestra.Services.IStatusService
    {
        public StatusService(Orchestra.Services.IStatusFilterService statusFilterService) { }
        public void Initialize(Orc.Controls.Services.IStatusRepresenter statusRepresenter) { }
        public void UpdateStatus(string status) { }
    }
    public class ThirdPartyNoticesService : Orchestra.Services.IThirdPartyNoticesService
    {
        public ThirdPartyNoticesService() { }
        public void Add(Orchestra.ThirdPartyNotice thirdPartyNotice) { }
        public System.Collections.Generic.List<Orchestra.ThirdPartyNotice> GetThirdPartyNotices() { }
    }
    public class ViewActivationService : Orchestra.Services.IViewActivationService
    {
        public ViewActivationService(Catel.MVVM.Views.IViewManager viewManager) { }
        public bool Activate(Catel.MVVM.IViewModel viewModel) { }
        public bool Activate(System.Type viewModelType) { }
    }
}
namespace Orchestra.Theming
{
    public interface IShellTheme
    {
        void ApplyTheme(Orc.Theming.ThemeInfo themeInfo);
    }
    public interface IThemeManager
    {
        void EnsureApplicationThemes(System.Reflection.Assembly assembly, bool createStyleForwarders = false);
        void EnsureApplicationThemes(string resourceDictionaryUri, bool createStyleForwarders = false);
        void EnsureApplicationThemes(System.Windows.ResourceDictionary resourceDictionary, bool createStyleForwarders = false);
        bool IsResourceDictionaryAvailable(string resourceDictionaryUri);
        void SynchronizeTheme();
    }
    public class ThemeManager : Orchestra.Theming.IThemeManager
    {
        protected bool _ensuredOrchestraThemes;
        public ThemeManager(Orc.Theming.IAccentColorService accentColorService, Orc.Theming.IBaseColorSchemeService baseColorSchemeService) { }
        public virtual void EnsureApplicationThemes(System.Reflection.Assembly assembly, bool createStyleForwarders = false) { }
        public virtual void EnsureApplicationThemes(string resourceDictionaryUri, bool createStyleForwarders = false) { }
        public virtual void EnsureApplicationThemes(System.Windows.ResourceDictionary resourceDictionary, bool createStyleForwarders = false) { }
        protected virtual void EnsureOrchestraTheme(bool createStyleForwarders) { }
        protected virtual System.Windows.ResourceDictionary GetTargetApplicationResourceDictionary() { }
        public virtual bool IsResourceDictionaryAvailable(string resourceDictionaryUri) { }
        public virtual void SynchronizeTheme() { }
    }
}
namespace Orchestra.Tooltips
{
    public class AdorneredTooltip : Orchestra.Tooltips.IAdorneredTooltip
    {
        public AdorneredTooltip(System.Windows.Documents.Adorner adorner, bool adornerLayerVisible) { }
        public bool AdornerLayerVisible { get; set; }
        public bool Visible { get; set; }
    }
    public interface IAdorneredTooltip
    {
        bool AdornerLayerVisible { get; set; }
        bool Visible { get; set; }
    }
    public interface IAdorneredTooltipFactory
    {
        Orchestra.Tooltips.IAdorneredTooltip Create(System.Windows.Documents.Adorner adornered, bool adornerLayerVisibility);
    }
    public class TextBlockAdorner : System.Windows.Documents.Adorner
    {
        public TextBlockAdorner(System.Windows.UIElement adornedElement, string text) { }
        public TextBlockAdorner(System.Windows.UIElement uiElement, Orchestra.Models.IHint hint) { }
        protected override System.Collections.IEnumerator LogicalChildren { get; }
        protected override int VisualChildrenCount { get; }
        protected override System.Windows.Size ArrangeOverride(System.Windows.Size finalSize) { }
        protected override System.Windows.Media.Visual GetVisualChild(int index) { }
        protected override System.Windows.Size MeasureOverride(System.Windows.Size constraint) { }
    }
}
namespace Orchestra.ViewModels
{
    public class AboutViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData AppIconProperty;
        public static readonly Catel.Data.PropertyData BuildDateTimeProperty;
        public static readonly Catel.Data.PropertyData CompanyLogoUriProperty;
        public static readonly Catel.Data.PropertyData CopyrightProperty;
        public static readonly Catel.Data.PropertyData CopyrightUrlProperty;
        public static readonly Catel.Data.PropertyData ImageSourceUrlProperty;
        public static readonly Catel.Data.PropertyData IsDebugLoggingEnabledProperty;
        public static readonly Catel.Data.PropertyData ShowLogButtonProperty;
        public static readonly Catel.Data.PropertyData TitleProperty;
        public static readonly Catel.Data.PropertyData UriInfoProperty;
        public static readonly Catel.Data.PropertyData VersionProperty;
        public AboutViewModel(Orchestra.Models.AboutInfo aboutInfo, Catel.Services.IProcessService processService, Catel.Services.IUIVisualizerService uiVisualizerService, Catel.Services.IMessageService messageService, Catel.Services.ILanguageService languageService, Orchestra.Changelog.IChangelogService changelogService) { }
        public System.Windows.Media.Imaging.BitmapSource AppIcon { get; }
        public string BuildDateTime { get; }
        public System.Uri CompanyLogoUri { get; }
        public string Copyright { get; }
        public string CopyrightUrl { get; }
        public Catel.MVVM.Command EnableDetailedLogging { get; }
        public string ImageSourceUrl { get; }
        public bool IsDebugLoggingEnabled { get; }
        public Catel.MVVM.Command OpenCopyrightUrl { get; }
        public Catel.MVVM.TaskCommand OpenLog { get; }
        public Catel.MVVM.Command OpenUrl { get; }
        public Catel.MVVM.TaskCommand ShowChangelog { get; }
        public bool ShowLogButton { get; }
        public Catel.MVVM.TaskCommand ShowSystemInfo { get; }
        public Catel.MVVM.TaskCommand ShowThirdPartyNotices { get; }
        public override string Title { get; set; }
        public Orchestra.Models.UriInfo UriInfo { get; }
        public string Version { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public class KeyboardMappingsCustomizationViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData CommandFilterProperty;
        public static readonly Catel.Data.PropertyData CommandsProperty;
        public static readonly Catel.Data.PropertyData SelectedCommandInputGestureProperty;
        public static readonly Catel.Data.PropertyData SelectedCommandNewInputGestureProperty;
        public static readonly Catel.Data.PropertyData SelectedCommandProperty;
        public KeyboardMappingsCustomizationViewModel(Orchestra.Services.IKeyboardMappingsService keyboardMappingsService, Catel.MVVM.ICommandManager commandManager, Orchestra.Services.ICommandInfoService commandInfoService, Catel.Services.ILanguageService languageService, Catel.Services.IMessageService messageService) { }
        public Catel.MVVM.TaskCommand Assign { get; }
        public string CommandFilter { get; set; }
        public Catel.Collections.FastObservableCollection<Orchestra.Models.ICommandInfo> Commands { get; }
        public Catel.MVVM.Command Remove { get; }
        public Catel.MVVM.TaskCommand Reset { get; }
        public string SelectedCommand { get; set; }
        public Catel.Windows.Input.InputGesture SelectedCommandInputGesture { get; }
        public Catel.Windows.Input.InputGesture SelectedCommandNewInputGesture { get; set; }
        public override string Title { get; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public class KeyboardMappingsOverviewViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData KeyboardMappingsProperty;
        public KeyboardMappingsOverviewViewModel(Catel.MVVM.ICommandManager commandManager, Orchestra.Services.ICommandInfoService commandInfoService, Catel.Services.IUIVisualizerService uiVisualizerService, Catel.Services.ILanguageService languageService, Catel.Services.IViewExportService viewExportService, Orchestra.Services.IKeyboardMappingsService keyboardMappingsService) { }
        public Catel.MVVM.TaskCommand Customize { get; }
        public System.Collections.Generic.List<Orchestra.Models.KeyboardMappings> KeyboardMappings { get; }
        public Catel.MVVM.TaskCommand Print { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public class MessageBoxViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ButtonProperty;
        public static readonly Catel.Data.PropertyData IconProperty;
        public static readonly Catel.Data.PropertyData MessageProperty;
        public static readonly Catel.Data.PropertyData ResultProperty;
        public MessageBoxViewModel(Catel.Services.IMessageService messageService, Orchestra.Services.IClipboardService clipboardService) { }
        public Catel.Services.MessageButton Button { get; set; }
        public Catel.MVVM.TaskCommand CancelCommand { get; }
        public Catel.MVVM.Command CopyToClipboard { get; }
        public Catel.MVVM.TaskCommand EscapeCommand { get; }
        public Catel.Services.MessageImage Icon { get; set; }
        public string Message { get; set; }
        public Catel.MVVM.TaskCommand NoCommand { get; }
        public Catel.MVVM.TaskCommand OkCommand { get; }
        public Catel.Services.MessageResult Result { get; set; }
        public Catel.MVVM.TaskCommand YesCommand { get; }
        protected override System.Threading.Tasks.Task CloseAsync() { }
        public void SetTitle(string title) { }
    }
    public class SplashScreenViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData CompanyLogoForSplashScreenUriProperty;
        public static readonly Catel.Data.PropertyData CompanyProperty;
        public static readonly Catel.Data.PropertyData ProducedByProperty;
        public static readonly Catel.Data.PropertyData VersionProperty;
        public SplashScreenViewModel(Orchestra.Services.IAboutInfoService aboutInfoService, Catel.Services.ILanguageService languageService) { }
        public string Company { get; }
        public System.Uri CompanyLogoForSplashScreenUri { get; }
        public string ProducedBy { get; }
        public string Version { get; }
        public static bool IsActive { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
        protected override System.Threading.Tasks.Task OnClosedAsync(bool? result) { }
    }
    public class SystemInfoViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData IsSystemInformationLoadedProperty;
        public static readonly Catel.Data.PropertyData SystemInfoProperty;
        public SystemInfoViewModel(Orc.SystemInfo.ISystemInfoService systemInfoService, Orchestra.Services.IClipboardService clipboardService) { }
        public Catel.MVVM.Command CopyToClipboard { get; }
        public bool IsSystemInformationLoaded { get; }
        public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> SystemInfo { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
    public class ThirdPartyNoticesViewModel : Catel.MVVM.ViewModelBase
    {
        public static readonly Catel.Data.PropertyData ExplanationProperty;
        public static readonly Catel.Data.PropertyData ThirdPartyNoticesProperty;
        public ThirdPartyNoticesViewModel(Orchestra.Services.IAboutInfoService aboutInfoService, Orchestra.Services.IThirdPartyNoticesService thirdPartyNoticesService) { }
        public string Explanation { get; }
        public System.Collections.Generic.List<Orchestra.ThirdPartyNotice> ThirdPartyNotices { get; }
        protected override System.Threading.Tasks.Task InitializeAsync() { }
    }
}
namespace Orchestra.Views
{
    public class AboutWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public AboutWindow() { }
        public AboutWindow(Orchestra.ViewModels.AboutViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class CrashWarningViewModel : Catel.MVVM.ViewModelBase
    {
        public CrashWarningViewModel(Orchestra.Services.IManageAppDataService manageAppDataService, Catel.Services.IMessageService messageService, Catel.Services.INavigationService navigationService, Catel.Services.ILanguageService languageService) { }
        public Catel.MVVM.TaskCommand BackupAndReset { get; set; }
        public Catel.MVVM.TaskCommand Continue { get; set; }
        public Catel.MVVM.TaskCommand ResetUserSettings { get; set; }
        public override string Title { get; }
        protected override System.Threading.Tasks.Task<bool> CancelAsync() { }
    }
    public class CrashWarningWindow : Catel.Windows.Window, System.Windows.Markup.IComponentConnector
    {
        public CrashWarningWindow() { }
        public void InitializeComponent() { }
    }
    public class KeyboardMappingsCustomizationView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public KeyboardMappingsCustomizationView() { }
        public void InitializeComponent() { }
    }
    public class KeyboardMappingsCustomizationWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public KeyboardMappingsCustomizationWindow() { }
        public KeyboardMappingsCustomizationWindow(Orchestra.ViewModels.KeyboardMappingsCustomizationViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class KeyboardMappingsOverviewView : Catel.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector
    {
        public KeyboardMappingsOverviewView() { }
        public void InitializeComponent() { }
    }
    public class KeyboardMappingsOverviewWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public KeyboardMappingsOverviewWindow() { }
        public KeyboardMappingsOverviewWindow(Orchestra.ViewModels.KeyboardMappingsOverviewViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class MessageBoxWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public MessageBoxWindow() { }
        public MessageBoxWindow(Orchestra.ViewModels.MessageBoxViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class SplashScreen : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public SplashScreen() { }
        public void InitializeComponent() { }
    }
    public class SystemInfoWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public SystemInfoWindow() { }
        public SystemInfoWindow(Orchestra.ViewModels.SystemInfoViewModel viewModel) { }
        public void InitializeComponent() { }
    }
    public class ThirdPartyNoticesWindow : Catel.Windows.DataWindow, System.Windows.Markup.IComponentConnector
    {
        public ThirdPartyNoticesWindow() { }
        public void InitializeComponent() { }
    }
}
namespace Orchestra.Windows
{
    public static class DisplayConfig { }
    public class DpiScale
    {
        public DpiScale() { }
        public double X { get; set; }
        public double Y { get; set; }
        public void SetScaleFromAbsolute(uint absoluteDpiX, uint absoluteDpiY) { }
        public override string ToString() { }
    }
    public class FixMaximize : System.Windows.DependencyObject
    {
        public static readonly System.Windows.DependencyProperty FixMaximizeProperty;
        public FixMaximize() { }
        public static bool GetFixMaximize(System.Windows.Window ribbonWindow) { }
        public static void SetFixMaximize(System.Windows.Window ribbonWindow, bool value) { }
    }
    public class MonitorInfo
    {
        public MonitorInfo() { }
        public string AdapterDeviceName { get; set; }
        public string Availability { get; set; }
        public string DeviceName { get; set; }
        public string DeviceNameFull { get; set; }
        public Orchestra.Windows.DpiScale DpiScale { get; set; }
        public string FriendlyName { get; set; }
        public string Id { get; set; }
        public bool IsPrimary { get; set; }
        public string ManufactureCode { get; set; }
        public System.Windows.Int32Rect MonitorArea { get; set; }
        public ushort ProductCodeId { get; set; }
        public string ScreenHeight { get; set; }
        public string ScreenWidth { get; set; }
        public System.Windows.Int32Rect WorkingArea { get; set; }
        public System.Windows.Rect GetDpiAwareResolution() { }
        public System.Windows.Rect GetDpiAwareWorkingArea() { }
        public static Orchestra.Windows.MonitorInfo[] GetAllMonitors(bool throwErrorsForWrongAppManifest = true) { }
        public static Orchestra.Windows.MonitorInfo GetMonitorFromWindow(System.Windows.Window window) { }
        public static Orchestra.Windows.MonitorInfo GetMonitorFromWindowHandle(System.IntPtr handle) { }
        public static Orchestra.Windows.MonitorInfo GetPrimaryMonitor() { }
        [System.Flags]
        public enum DpiAwareness
        {
            Unaware = 0,
            System = 1,
            ProcessPerMonitor = 2,
        }
        [System.Flags]
        public enum DpiAwarenessContext
        {
            Unaware = 0,
            System = 1,
            ProcessPerMonitor = 2,
            ProcessPerMonitorV2 = 3,
            UnawareGdiScaled = 4,
        }
        public enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }
    }
    public sealed class Taskbar
    {
        public Taskbar() { }
        public bool AlwaysOnTop { get; }
        public bool AutoHide { get; }
        public System.Drawing.Rectangle Bounds { get; }
        public System.Drawing.Point Location { get; }
        public Orchestra.Windows.TaskbarPosition Position { get; }
        public System.Drawing.Size Size { get; }
    }
    public enum TaskbarPosition
    {
        Unknown = -1,
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3,
    }
    public static class WindowExtensions
    {
        public static void ApplyApplicationIcon(this System.Windows.Window window) { }
    }
}