using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Caliberweb.Core.Serialization;

namespace Caliberweb.Core.Licensing
{
    class LicensingService<T> : ILicensingService<T> where T: ILicense
    {
        private readonly string publicKey;
        private readonly ILicenseCreator<T> creator;
        private readonly IDataSerializer serializer;
        private readonly string privateKey;
        private readonly IByteCodec byteCodec;

        internal LicensingService(string privateKey, string publicKey, ILicenseCreator<T> creator, IDataSerializer serializer)
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
            this.creator = creator;
            this.serializer = serializer;

            byteCodec = ByteCodec.Null;
        }

        public string GenerateLicense(string licensee, LicenseType type, DateTime expiration)
        {
            var generator = new LicenseGenerator<T>(privateKey, creator);

            T license = generator.Generate(licensee, type, expiration);

            return byteCodec.Encode(serializer.Serialize(license));
        }

        public T ValidateLicense(string licensePath)
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.FromXmlString(publicKey);

                var text = File.ReadAllText(licensePath);

                var bytes = byteCodec.Decode(text);

                var license = serializer.Deserialize<T>(bytes.ToArray());

                if (!LicenseGenerator<T>.VerifySignature(license, provider, creator))
                {
                    throw new InvalidDataException("License is invalid");
                }

                return license;
            }

        }
    }

    public static class LicensingService
    {
        private const int DEFAULT_KEY_SIZE = 2048;

        public static ILicensingService<T> Create<T>(string privateKey, string publicKey, ILicenseCreator<T> creator) where T : ILicense
        {
            return Create(privateKey, publicKey, creator, Serializers.Json);
        }

        public static ILicensingService<T> Create<T>(string privateKey, string publicKey, ILicenseCreator<T> creator, IDataSerializer serializer) where T : ILicense
        {
            return new LicensingService<T>(privateKey, publicKey, creator, serializer);
        }

        public static void GenerateKeypair(FileInfo publicKey, FileInfo privateKey)
        {
            GenerateKeypair(publicKey, privateKey, DEFAULT_KEY_SIZE);
        }

        public static void GenerateKeypair(FileInfo publicKey, FileInfo privateKey, int keySize)
        {
            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                File.WriteAllText(privateKey.FullName, provider.ToXmlString(true));
                File.WriteAllText(publicKey.FullName, provider.ToXmlString(false));
            }
        }
    }
}