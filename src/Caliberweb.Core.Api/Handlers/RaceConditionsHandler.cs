using Caliberweb.Core.Api.Resources;
using Caliberweb.Core.Weather;

using OpenRasta.Web;

namespace Caliberweb.Core.Api.Handlers
{
    public class RaceConditionsHandler
    {
        public OperationResult Get(int zipCode)
        {
            var request = new YahooRequest(zipCode);

            Conditions conditions = request.GetConditions();

            Atmosphere atmosphere = conditions.Atmosphere;
            Location location = conditions.Location;

            Elevation elevation = Elevation.Get(location.Longitude, location.Latitude);

            double da = DensityAltitude.Calculate(elevation.Feet, atmosphere);

            var race = new RaceConditions
            {
                DensityAltitude = da,
                Temperature = atmosphere.Temperature,
                Elevation = elevation.Feet,
                Humidity = atmosphere.Humidity,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Pressure = atmosphere.Pressure,                
            };

            return new OperationResult.OK(race);
        }
    }
}