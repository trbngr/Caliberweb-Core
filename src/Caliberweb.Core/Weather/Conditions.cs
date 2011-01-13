namespace Caliberweb.Core.Weather
{
    public class Conditions
    {
        public static Conditions Empty = new Conditions(Atmosphere.Empty, Location.Center);

        public Conditions(Atmosphere atmosphere, Location location)
        {
            Atmosphere = atmosphere;
            Location = location;
        }

        public Atmosphere Atmosphere { get; private set; }

        public Location Location { get; private set; }
    }
}