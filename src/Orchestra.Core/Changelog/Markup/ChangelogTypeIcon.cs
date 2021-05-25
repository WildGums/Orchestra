namespace Orchestra.Changelog.Markup
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class ChangelogTypeIcon : Catel.Windows.Markup.UpdatableMarkupExtension
    {
        public ChangelogType? ChangelogType { get; set; }

        public BindingBase ChangelogTypeBinding { get; set; }

        private static readonly DependencyProperty ChangelogTypeBindingBindingSinkProperty = DependencyProperty.RegisterAttached("ChangelogTypeBindingBindingSink",
            typeof(object), typeof(ChangelogTypeIcon), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        protected override object ProvideDynamicValue(IServiceProvider serviceProvider)
        {
            var application = Application.Current;
            if (application is null)
            {
                return null;
            }

            var changeLogType = ChangelogType;

            var changelogTypeBinding = ChangelogTypeBinding;
            if (changelogTypeBinding is not null)
            {
                if (TargetObject is DependencyObject targetObject)
                {
                    BindingOperations.SetBinding(targetObject, ChangelogTypeBindingBindingSinkProperty, changelogTypeBinding);

                    var changelogTypeBindingValue = targetObject.GetValue(ChangelogTypeBindingBindingSinkProperty);
                    if (changelogTypeBindingValue is ChangelogType boundChangelogType)
                    {
                        changeLogType = boundChangelogType;
                    }
                }
            }

            var keyName = $"{changeLogType}DataTemplate";

            var resource = application.TryFindResource(keyName);
            if (resource is not null)
            {
                return resource;
            }

            return base.ProvideDynamicValue(serviceProvider);
        }

        protected override void OnTargetObjectLoaded()
        {
            base.OnTargetObjectLoaded();

            UpdateValue();
        }
    }
}
