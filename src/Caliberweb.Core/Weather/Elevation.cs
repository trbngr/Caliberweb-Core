using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

using Caliberweb.Core.USGS;

namespace Caliberweb.Core.Weather
{
    public struct Elevation
    {
        private Elevation(double feet) : this()
        {
            Feet = feet;
        }

        public double Feet { get; private set; }

        public static Elevation Get(double longitude, double latitude)
        {
            Binding binding = new BasicHttpBinding();
            var address = new EndpointAddress("http://gisdata.usgs.net/xmlwebservices2/elevation_service.asmx");

            var client = new Elevation_ServiceSoapClient(binding, address);

            XmlNode node = client.getElevation(longitude.ToString(), latitude.ToString(), "", "", "true");

            client.Close();

            double feet = double.Parse(node.InnerText);

            return new Elevation(feet);
        }
    }
}