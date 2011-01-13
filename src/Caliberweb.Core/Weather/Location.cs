using System.Runtime.Serialization;

namespace Caliberweb.Core.Weather
{
    [DataContract]
    public class Location
    {
        public static Location Center = new Location(Weather.Latitude.Center, Weather.Longitude.Center);

        public Location(Latitude latitude, Longitude longitude)
        {
            Longitude = longitude.Value;
            Latitude = latitude.Value;
        }

        [DataMember]
        public double Longitude { get; private set; }

        [DataMember]
        public double Latitude { get; private set; }
    }
}