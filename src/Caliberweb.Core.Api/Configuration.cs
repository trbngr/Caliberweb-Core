using Caliberweb.Core.Api.Handlers;
using Caliberweb.Core.Api.Resources;
using OpenRasta.Configuration;

namespace Caliberweb.Core.Api
{
    public class Configuration : IConfigurationSource
    {
        public void Configure()
        {
            using(OpenRastaConfiguration.Manual)
            {
                ResourceSpace.Has.ResourcesOfType<RaceConditions>()
                    .AtUri("/race/conditions/{zipCode}")
                    .HandledBy<RaceConditionsHandler>()
                    .AsJsonDataContract();
            }
        }
    }
}