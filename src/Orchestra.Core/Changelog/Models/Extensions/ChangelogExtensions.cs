namespace Orchestra.Changelog
{
    using System.Linq;
    using Catel;

    public static class ChangelogExtensions
    {
        public static Changelog GetDelta(this Changelog changelog1, Changelog changelog2)
        {
            Argument.IsNotNull(() => changelog1);
            Argument.IsNotNull(() => changelog2);

            // Note: we do simple delta comparsion, only the ones we added should be part

            var delta = new Changelog();

            foreach (var item in changelog2.Items)
            {
                if (changelog1.Items.Any(x => x.Name.Equals(item.Name)))
                {
                    continue;
                }

                delta.Items.Add(item);
            }

            return delta;
        }
    }
}
