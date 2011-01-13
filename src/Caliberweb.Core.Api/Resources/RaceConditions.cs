using System.Runtime.Serialization;

namespace Caliberweb.Core.Api.Resources
{
    [DataContract]
    public class RaceConditions
    {
        [DataMember]
        public double Longitude { get; set; }

        [DataMember]
        public double Latitude { get; set; }

        [DataMember]
        public double DensityAltitude { get; set; }

        [DataMember]
        public double Elevation { get; set; }

        [DataMember]
        public double Temperature { get; set; }

        [DataMember]
        public double Humidity { get; set; }

        [DataMember]
        public double Pressure { get; set; }
    }
}