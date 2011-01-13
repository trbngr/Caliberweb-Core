using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace Caliberweb.Core.Licensing
{
    class LicenseGenerator<T> where T : ILicense
    {
        private readonly string privateKey;
        private readonly ILicenseCreator<T> decorator;

        public LicenseGenerator(string privateKey, ILicenseCreator<T> decorator)
        {
            this.privateKey = privateKey;
            this.decorator = decorator;
        }

        private byte[] GenerateSignature(T license)
        {
            SignedXml signature;
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(privateKey);

                XmlDocument doc = GenerateXmlDocument(license, decorator);

                signature = new SignedXml(doc) { SigningKey = provider };

                var reference = new Reference { Uri = "" };

                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());

                signature.AddReference(reference);
                signature.ComputeSignature();
            }

            return Encoding.UTF8.GetBytes(signature.GetXml().OuterXml);
        }

        public T Generate(string licensee, LicenseType type, DateTime expiration)
        {
            T concreteLicense = decorator.CreateLicense(new LicenseBase(licensee, type, expiration));

            concreteLicense.Signature = GenerateSignature(concreteLicense);

            return concreteLicense;
        }

        private static XmlDocument GenerateXmlDocument(T license, ILicenseCreator<T> decorator)
        {
            return new LicenseDocument<T>(decorator).Create(license);
        }

        public static bool VerifySignature(T license, AsymmetricAlgorithm algorithm, ILicenseCreator<T> decorator)
        {
            XmlDocument doc = GenerateXmlDocument(license, decorator);

            var xml = new SignedXml(doc);

            var signatureElement = new XmlDocument();
            signatureElement.LoadXml(Encoding.UTF8.GetString(license.Signature));

// ReSharper disable AssignNullToNotNullAttribute
            xml.LoadXml(signatureElement.DocumentElement);
// ReSharper restore AssignNullToNotNullAttribute

            bool result = xml.CheckSignature(algorithm);

            return result;
        }
    }
}