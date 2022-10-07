namespace Orchestra.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Windows;
    using Catel;

    public class HintsProvider : IHintsProvider
    {
        private readonly Dictionary<Type, List<IHint>> _hints = new Dictionary<Type, List<IHint>>();

        public void AddHint<TControlType>(string hintText, Expression<Func<object>> userControlName)
        {
            ArgumentNullException.ThrowIfNull(hintText);
            ArgumentNullException.ThrowIfNull(userControlName);

            var controlName = string.Empty;
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
            ArgumentNullException.ThrowIfNull(element);

            if (_hints.TryGetValue(element.GetType(), out var hints))
            {
                return hints.ToArray();
            }

            return Array.Empty<IHint>();
        }
    }
}
