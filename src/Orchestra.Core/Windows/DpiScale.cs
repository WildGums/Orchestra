namespace Orchestra.Windows
{
    public class DpiScale
    {
        private const double BasicAbsoluteDpi = 96;

        public double X { get; set; } = BasicAbsoluteDpi;
        public double Y { get; set; } = BasicAbsoluteDpi;

        public void SetScaleFromAbsolute(uint absoluteDpiX, uint absoluteDpiY)
        {
            X = absoluteDpiX / BasicAbsoluteDpi;
            Y = absoluteDpiY / BasicAbsoluteDpi;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y}";
        }
    }
}
