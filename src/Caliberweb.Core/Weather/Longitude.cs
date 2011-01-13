using System;

namespace Caliberweb.Core.Weather
{
    public struct Longitude
    {
        public static Longitude Center = new Longitude(0);
        private const int MAX = 180;

        public Longitude(double value) : this()
        {
            EnsureValidValue(value);
            Value = value;
        }

        private static void EnsureValidValue(double value)
        {
            if(value < -1*MAX || value > MAX)
                throw new ArgumentOutOfRangeException("value", "Longitude must be in the range of -180 to 180");
        }

        public double Value { get; private set; }
    }
}