using System;

namespace Caliberweb.Core.Weather
{
    public struct Latitude
    {
        private const int MAX = 90;
        public static Latitude Center = new Latitude(0);

        public Latitude(double value)
            : this()
        {
            EnsureValidValue(value);
            Value = value;
        }

        public double Value { get; private set; }

        private static void EnsureValidValue(double value)
        {
            if (value < -1*MAX || value > MAX)
            {
                throw new ArgumentOutOfRangeException("value", "Latitude must be in the range of -90 to 90");
            }
        }
    }
}