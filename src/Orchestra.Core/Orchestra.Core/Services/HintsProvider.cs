// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HintsProvider.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
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
        private readonly Dictionary<Type, IList<IHint>> _hints = new Dictionary<Type, IList<IHint>>();
        #endregion

        #region Methods
        public void AddHint<TControlType>(string hintText, Expression<Func<object>> userControlName)
        {
            string controlName = string.Empty;
            if (userControlName != null)
            {
                controlName = ExpressionHelper.GetPropertyName(userControlName);
            }

            AddHint<TControlType>(new Hint(hintText, controlName));
        }

        public void AddHint<TControlType>(IHint hint)
        {
            Argument.IsNotNull(() => hint);

            var type = typeof(TControlType);
            if (!_hints.ContainsKey(type))
            {
                _hints.Add(type, new List<IHint>());
            }

            _hints[type].Add(hint);
        }

        public IList<IHint> GetHintsFor(FrameworkElement element)
        {
            IList<IHint> hints = null;

            if (_hints.TryGetValue(element.GetType(), out hints))
            {
                return hints;
            }

            return null;
        }
        #endregion
    }
}