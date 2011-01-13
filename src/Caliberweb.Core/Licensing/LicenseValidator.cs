using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Caliberweb.Core.Serialization;

namespace Caliberweb.Core.Licensing
{
    class LicenseValidator<T> where T: ILicense
    {
        private readonly IDataSerializer serializer;
        private readonly IByteCodec byteCodec;
        private readonly string publicKey;

        public LicenseValidator(string publicKey, IDataSerializer serializer, IByteCodec byteCodec)
        {
            this.serializer = serializer;
            this.byteCodec = byteCodec;
            this.publicKey = publicKey;
        }

        public T ValidateLicense(string licensePath, ILicenseCreator<T> decorator)
        {
            using(var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(publicKey);

                var text = File.ReadAllText(licensePath);

                var bytes = byteCodec.Decode(text);
                
                var license = serializer.Deserialize<T>(bytes.ToArray());

                if (!LicenseGenerator<T>.VerifySignature(license, provider, decorator))
                {
                    throw new InvalidDataException("License is invalid");
                }

                return license;
            }
        }
    }
}