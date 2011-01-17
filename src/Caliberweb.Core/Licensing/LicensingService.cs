using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Caliberweb.Core.Serialization;

namespace Caliberweb.Core.Licensing
{
    class LicensingService<T> : ILicensingService<T> where T: ILicense
    {
        private readonly IKeyPair keyPair;
        private readonly ILicenseCreator<T> creator;
        private readonly IDataSerializer serializer;
        private readonly IByteCodec byteCodec;

        internal LicensingService(IKeyPair keyPair, ILicenseCreator<T> creator, IDataSerializer serializer)
        {
            this.keyPair = keyPair;
            this.creator = creator;
            this.serializer = serializer;

            byteCodec = ByteCodec.Hex;
        }

        public string GenerateLicense(string licensee, LicenseType type, DateTime expiration)
        {
            var generator = new LicenseGenerator<T>(keyPair.Private, creator);

            T license = generator.Generate(licensee, type, expiration);

            var bytes = serializer.Serialize(license);

            return byteCodec.Encode(bytes);
        }

        public T ValidateLicense(string license)
        {
            using (var provider = new RSACryptoServiceProvider())
            {
                provider.ImportCspBlob(keyPair.Public);

                var bytes = byteCodec.Decode(license);

                var instance = serializer.Deserialize<T>(bytes.ToArray());

                if (!LicenseGenerator<T>.VerifySignature(instance, provider, creator))
                {
                    throw new InvalidDataException("License is invalid");
                }

                return instance;
            }

        }
    }

    public static class LicensingService
    {
        private static readonly IDataSerializer defautltSerializer = Serializers.Json;
        private const int DEFAULT_KEY_SIZE = 2048;

        public static ILicensingService<T> Create<T>(IKeyPair keyPair, ILicenseCreator<T> creator) where T : ILicense
        {
            return Create(keyPair, defautltSerializer, creator);
        }

        public static ILicensingService<T> Create<T>(IKeyPair keyPair, IDataSerializer serializer, ILicenseCreator<T> creator) where T : ILicense
        {
            return new LicensingService<T>(keyPair, creator, serializer);
        }

        public static IKeyPair GenerateKeypair()
        {
            return GenerateKeypair(DEFAULT_KEY_SIZE);
        }

        public static IKeyPair GenerateKeypair(int keySize)
        {
            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                return new KeyPair
                {
                    Public = provider.ExportCspBlob(false),
                    Private = provider.ExportCspBlob(true)
                };
            }
        }

    }
}