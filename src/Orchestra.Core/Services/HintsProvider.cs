// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HintsProvider.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Windows;
    using Catel;
    using Models;

    public class HintsProvider : IHintsProvider
    {
        #region Fields
        private readonly Dictionary<Type, List<IHint>> _hints = new Dictionary<Type, List<IHint>>();
        #endregion

        #region Methods
        public void AddHint<TControlType>(string hintText, Expression<Func<object>> userControlName)
        {
            string controlName = string.Empty;
            if (userControlName is not null)
            {
                controlName = ExpressionHelper.GetPropertyName(userControlName);
            }

            AddHint<TControlType>(new Hint(hintText, controlName));
        }

        public void AddHint<TControlType>(IHint hint)
        {
            ArgumentNullException.ThrowIfNull(hint);

            var type = typeof(TControlType);
            if (!_hints.ContainsKey(type))
            {
                _hints.Add(type, new List<IHint>());
            }

            _hints[type].Add(hint);
        }

        public IHint[] GetHintsFor(FrameworkElement element)
        {
            if (_hints.TryGetValue(element.GetType(), out var hints))
            {
                return hints.ToArray();
            }

            return Array.Empty<IHint>();
        }
        #endregion
    }
}
