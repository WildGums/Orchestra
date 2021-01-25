namespace Orchestra.Services
{
    using System.Collections.Generic;
    using System.Windows.Input;

    public class KeyboardMappingsAllowedKeysService : IKeyboardMappingsAllowedKeysService
    {
        protected readonly HashSet<Key> IgnoredKeys = new HashSet<Key>();

        public KeyboardMappingsAllowedKeysService()
        {
            IgnoredKeys.Add(Key.None);
            IgnoredKeys.Add(Key.Cancel);
            IgnoredKeys.Add(Key.Back);
            IgnoredKeys.Add(Key.Tab);
            IgnoredKeys.Add(Key.LineFeed);
            IgnoredKeys.Add(Key.Clear);
            IgnoredKeys.Add(Key.Return);
            IgnoredKeys.Add(Key.Enter);
            IgnoredKeys.Add(Key.Pause);
            IgnoredKeys.Add(Key.Capital);
            IgnoredKeys.Add(Key.CapsLock);
            IgnoredKeys.Add(Key.KanaMode);
            IgnoredKeys.Add(Key.HangulMode);
            IgnoredKeys.Add(Key.JunjaMode);
            IgnoredKeys.Add(Key.FinalMode);
            IgnoredKeys.Add(Key.HanjaMode);
            IgnoredKeys.Add(Key.KanjiMode);
            IgnoredKeys.Add(Key.Space);

            IgnoredKeys.Add(Key.System);

            IgnoredKeys.Add(Key.LeftCtrl);
            IgnoredKeys.Add(Key.RightCtrl);
            IgnoredKeys.Add(Key.LeftAlt);
            IgnoredKeys.Add(Key.RightAlt);
            IgnoredKeys.Add(Key.LeftShift);
            IgnoredKeys.Add(Key.RightShift);
        }

        public virtual bool IsAllowed(Key key)
        {
            if (IgnoredKeys.Contains(key))
            {
                return false;
            }

            return true;
        }
    }
}
