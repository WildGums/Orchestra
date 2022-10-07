namespace Orchestra
{
    using System;

    public class Hint : IHint
    {
        public Hint(string text, string controlName)
        {
            ArgumentNullException.ThrowIfNull(text);
            ArgumentNullException.ThrowIfNull(controlName);

            Text = text;
            ControlName = controlName;
        }

        public string Text { get; private set; }
        public string ControlName { get; private set; }
    }
}
