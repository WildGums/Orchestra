// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkElementExtensions.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra
{
    using System.Windows;
    using System.Windows.Interactivity;
    using Catel;
    using Catel.IoC;

    public static class FrameworkElementExtensions
    {
        public static TBehavior ApplyBehavior<TBehavior>(this DependencyObject dependencyObject)
            where TBehavior : Behavior
        {
            Argument.IsNotNull(() => dependencyObject);

            var behaviors = Interaction.GetBehaviors(dependencyObject);

            var typeFactory = dependencyObject.GetTypeFactory();
            var behavior = typeFactory.CreateInstanceWithParametersAndAutoCompletion<TBehavior>();

            behaviors.Add(behavior);

            return behavior;
        }
    }
}