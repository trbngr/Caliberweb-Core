using System.Xml;

namespace Caliberweb.Core.Licensing
{
    class LicenseDocument<T> : ILicenseDocument where T: ILicense
    {
        private readonly ILicenseCreator<T> creator;
        private XmlElement node;
        private XmlDocument doc;

        public LicenseDocument(ILicenseCreator<T> creator)
        {
            this.creator = creator;
        }

        public XmlDocument Create(T license)
        {
            doc = new XmlDocument();
            node = doc.CreateElement("license");

            XmlElement licensee = doc.CreateElement("licensee");
            licensee.InnerText = license.Licensee;

            XmlElement type = doc.CreateElement("type");
            type.InnerText = license.Type.ToString();

            XmlElement expiration = doc.CreateElement("expiration");
            expiration.InnerText = license.Expires.ToString();

            node.AppendChild(licensee);
            node.AppendChild(type);
            node.AppendChild(expiration);

            creator.AlterDocument(this, license);

            doc.AppendChild(node);

            return doc;
        }

        public void AppendData(string name, string text)
        {
            var element = doc.CreateElement(name);
            element.InnerText = text;
            node.AppendChild(element);
        }
    }
}