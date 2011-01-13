using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Caliberweb.Core.Weather
{
    public class YahooRequest
    {
        private const string URI_MASK =
            @"http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20weather.forecast%20where%20location%3D{0}&diagnostics=true";
        
        private readonly Uri uri;

        public YahooRequest(int zip)
        {
            uri = new Uri(string.Format(URI_MASK, zip));
        }

        public Conditions GetConditions()
        {
            var doc = XDocument.Load(uri.AbsoluteUri);
            doc.Save("results.xml");

            const string WEATHER = "{http://xml.weather.yahoo.com/ns/rss/1.0}";
            const string GEO = "{http://www.w3.org/2003/01/geo/wgs84_pos#}";
            const string ATMOSPHERE = "{0}atmosphere";
            const string CONDITION = "{0}condition";
            const string LATITUDE = "{0}lat";
            const string LONGITUDE = "{0}long";
            var atmosphere = doc.Descendants(string.Format(ATMOSPHERE, WEATHER)).FirstOrDefault();

            var hAttribute = atmosphere.Attribute("humidity");
            var pAttribute = atmosphere.Attribute("pressure");

            if (hAttribute == null || pAttribute == null)
            {
                return Conditions.Empty;
            }

            var h = double.Parse(hAttribute.Value);
            var p = double.Parse(pAttribute.Value);

            XElement condition = doc.Descendants(string.Format(CONDITION, WEATHER)).FirstOrDefault();

            var tAttribute = condition.Attribute("temp");
            if (tAttribute == null)
            {
                return Conditions.Empty;
            }

            var t = double.Parse(tAttribute.Value);

            XElement latitude = doc.Descendants(string.Format(LATITUDE, GEO)).FirstOrDefault();
            XElement longitude = doc.Descendants(string.Format(LONGITUDE, GEO)).FirstOrDefault();

            var lat = new Latitude(double.Parse(latitude.Value));
            var lon = new Longitude(double.Parse(longitude.Value));

            var a = new Atmosphere(t, h, p);
            var l = new Location(lat, lon);
            
            return new Conditions(a, l);
        }

//        private static  XmlNamespaceManager GetNamespaces(XmlNameTable table)
//        {
//            var manager = new XmlNamespaceManager(table);
//
//            manager.AddNamespace("yahoo", "http://www.yahooapis.com/v1/base.rng");
//            manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");
//            manager.AddNamespace("geo", "http://www.w3.org/2003/01/geo/wgs84_pos#");
//
//            return manager;
//        }
    }

    public class Atmosphere
    {
        public static Atmosphere Empty
        {
            get
            {
                return new Atmosphere(0, 0, 0);
            }
        }

        internal Atmosphere(double temperature, double humidity, double pressure)
        {
            Temperature = temperature;
            Humidity = humidity;
            Pressure = pressure;
        }

        public double Temperature { get; private set; }

        public double Humidity { get; private set; }

        public double Pressure { get; private set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Temperature: {0}", Temperature);
            builder.AppendLine();
            builder.AppendFormat("Humidity: {0}", Humidity);
            builder.AppendLine();
            builder.AppendFormat("Pressure: {0}", Pressure);

            return builder.ToString();
        }
    }
}