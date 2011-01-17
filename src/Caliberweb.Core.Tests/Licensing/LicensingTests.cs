// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

using System;
using System.IO;
using System.Xml;

using Caliberweb.Core.Serialization;

using NUnit.Framework;

namespace Caliberweb.Core.Licensing
{
    [TestFixture]
    public class LicensingTests
    {
        #region Setup/Teardown

        [SetUp]
        public void BeforeEachTest()
        {
            serializer = Serializers.Xml;

            //Generate keypair
            var keyPair = LicensingService.GenerateKeypair();

            //note:only generate the key pair once per application and store the keys somewhere secure.
//            keyPair.Save(fileinfo, serializer);
//            keyPair = KeyPair.Load(fileinfo, serializer)

            //customised license
            var number = Rand.Next();
            var name = Rand.String.NextText(1, 3);

            creator = new MyTestLicenseCreator(number, name);

            //create the licensing service
            service = LicensingService.Create(keyPair, serializer, creator);

            //generate license
            var licensee = Rand.String.NextText(1, 3);
            const LicenseType LICENSE_TYPE = LicenseType.Full; 
            var expiration = DateTime.MaxValue.AddYears(-50);

            license = service.GenerateLicense(licensee, LICENSE_TYPE, expiration);
        }

        #endregion

        private ILicenseCreator<MyTestLicense> creator;
        private ILicensingService<MyTestLicense> service;

        private IDataSerializer serializer;
        private string license;

        [Test]
        public void LicenseIsWrittenAsValid()
        {
            Assert.DoesNotThrow(() => service.ValidateLicense(license));
        }

        [Test]
        public void LicenseIsInvalidatedIfAltered()
        {
            //alter license file
            var xml = new XmlDocument();
            xml.LoadXml(license);

            var nsm = new XmlNamespaceManager(xml.NameTable);
            nsm.AddNamespace("c", xml.DocumentElement.NamespaceURI);

            var node = xml.SelectSingleNode("//c:name", nsm);

            node.InnerText = Rand.String.NextWord();

            license = xml.OuterXml;

            //assert
            Assert.Throws<InvalidDataException>(() => service.ValidateLicense(license));
        }
    }
}

// ReSharper restore PossibleNullReferenceException
// ReSharper restore AssignNullToNotNullAttribute