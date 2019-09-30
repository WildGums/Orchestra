namespace Orchestra.Themes
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Catel.Logging;
    using MahApps.Metro;
    using Orc.Controls;

    public class MahAppsShellTheme : IShellTheme
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public ResourceDictionary CreateResourceDictionary(ThemeInfo themeInfo)
        {
            var resourceDictionary = new ResourceDictionary();

            resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(new GradientStopCollection(new[]
            {
                    new GradientStop(Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor), 0),
                    new GradientStop(Orc.Controls.ThemeHelper.GetThemeColor(Orc.Controls.ThemeColorStyle.AccentColor3), 1)
            }), new Point(0.001, 0.5), new Point(1.002, 0.5)));

            resourceDictionary.Add("CheckmarkFill", Orc.Controls.ThemeHelper.GetThemeColorBrush());
            resourceDictionary.Add("RightArrowFill", Orc.Controls.ThemeHelper.GetThemeColorBrush());

            resourceDictionary.Add("IdealForegroundColor", Colors.White);
            resourceDictionary.Add("IdealForegroundColorBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush());
            resourceDictionary.Add("IdealForegroundDisabledBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush(0.4d));
            resourceDictionary.Add("AccentSelectedColorBrush", Colors.White.GetSolidColorBrush());

            resourceDictionary.Add("MetroDataGrid.HighlightBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush());
            resourceDictionary.Add("MetroDataGrid.HighlightTextBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush());
            resourceDictionary.Add("MetroDataGrid.MouseOverHighlightBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor3));
            resourceDictionary.Add("MetroDataGrid.FocusBorderBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush());
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightBrush", Orc.Controls.ThemeHelper.GetThemeColorBrush(Orc.Controls.ThemeColorStyle.AccentColor2));
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightTextBrush", ((Color)resourceDictionary["IdealForegroundColor"]).GetSolidColorBrush());

            return resourceDictionary;
        }

        public void ApplyTheme(ThemeInfo themeInfo)
        {
            //<Color x:Key="HighlightColor">
            //    #800080
            //</Color>
            //<Color x:Key="AccentColor">
            //    #CC800080
            //</Color>
            //<Color x:Key="AccentColor2">
            //    #99800080
            //</Color>
            //<Color x:Key="AccentColor3">
            //    #66800080
            //</Color>
            //<Color x:Key="AccentColor4">
            //    #33800080
            //</Color>

            //<SolidColorBrush x:Key="HighlightBrush" Color="{StaticResource HighlightColor}" />
            //<SolidColorBrush x:Key="AccentColorBrush" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="AccentColorBrush2" Color="{StaticResource AccentColor2}" />
            //<SolidColorBrush x:Key="AccentColorBrush3" Color="{StaticResource AccentColor3}" />
            //<SolidColorBrush x:Key="AccentColorBrush4" Color="{StaticResource AccentColor4}" />
            //<SolidColorBrush x:Key="WindowTitleColorBrush" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="AccentSelectedColorBrush" Color="White" />
            //<LinearGradientBrush x:Key="ProgressBrush" EndPoint="0.001,0.5" StartPoint="1.002,0.5">
            //    <GradientStop Color="{StaticResource HighlightColor}" Offset="0" />
            //    <GradientStop Color="{StaticResource AccentColor3}" Offset="1" />
            //</LinearGradientBrush>
            //<SolidColorBrush x:Key="CheckmarkFill" Color="{StaticResource AccentColor}" />
            //<SolidColorBrush x:Key="RightArrowFill" Color="{StaticResource AccentColor}" />

            //<Color x:Key="IdealForegroundColor">
            //    Black
            //</Color>
            //<SolidColorBrush x:Key="IdealForegroundColorBrush" Color="{StaticResource IdealForegroundColor}" />

            // Theme is always the 0-index of the resources
            var application = Application.Current;
            var applicationResources = application.Resources;
            var resourceDictionary = CreateResourceDictionary(themeInfo);

            var applicationTheme = ThemeManager.AppThemes.First(x => string.Equals(x.Name, "BaseLight"));

            // Insert to get the best MahApps performance (when looking up themes)
            applicationResources.MergedDictionaries.Remove(resourceDictionary);
            applicationResources.MergedDictionaries.Insert(2, applicationTheme.Resources);

            Log.Debug("Applying theme to MahApps");

            var newAccent = new Accent
            {
                Name = "Runtime accent (Orchestra)",
                Resources = resourceDictionary
            };

            ThemeManager.ChangeAppStyle(application, newAccent, applicationTheme);

            // Note: important to add the resources dictionary *after* changing the app style, but then insert at the top 
            // so MahApps theme detection performance is best
            applicationResources.MergedDictionaries.Insert(1, resourceDictionary);
        }
    }
}
