using System;
using System.IO;
using System.Runtime.Serialization;

using Caliberweb.Core;
using Caliberweb.Core.Extensions;
using Caliberweb.Core.Licensing;
using Caliberweb.Core.Serialization;

using ConsoleDriver.Internal;

using log4net;

namespace ConsoleDriver
{
    internal class Runner : IRunner, ILicenseCreator<CaliberwebLicense>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (Runner));
        private readonly ConsoleReader reader;
        private IDataSerializer serializer;

        public Runner(ConsoleReader reader)
        {
            this.reader = reader;
        }

        #region ILicenseCreator<CaliberwebLicense> Members

        public CaliberwebLicense CreateLicense(ILicense license)
        {
            return new CaliberwebLicense(license)
            {
                HomeTrack = reader.GetString("home track")
            };
        }

        public void AlterDocument(ILicenseDocument document, CaliberwebLicense license)
        {
            document.AppendData("track", license.HomeTrack);
        }

        #endregion

        #region IRunner Members

        public void Run()
        {
            const string LICENSE_FILE = "license.txt";
            serializer = Serializers.Xml;

            FileInfo keyPairFile = "keys.txt".ToLocalFileInfo();

            //            var keyPair = LicensingService.GenerateKeypair();        
            //            keyPair.Save(keyPairFile, serializer);
            IKeyPair keyPair = KeyPair.Load(keyPairFile, serializer);

            ILicensingService<CaliberwebLicense> service = LicensingService.Create(keyPair, serializer, this);

            bool running = true;
            while (running)
            {
                Generate(service, LICENSE_FILE);
                Validate(service, LICENSE_FILE);

                running = reader.Confirm("Run again?");
            }
        }

        #endregion

        private void Generate(ILicensingService<CaliberwebLicense> service, string licenseFile)
        {
            if (reader.Confirm("Generate new license?"))
            {
                string license = service.GenerateLicense(reader.GetString("Licensee"),
                                                         LicenseType.Full,
                                                         DateTime.MaxValue.AddYears(-10));

                File.WriteAllText(licenseFile, license);
            }
        }

        private void Validate(ILicensingService<CaliberwebLicense> service, string licenseFile)
        {
            if (reader.Confirm("Validate License?"))
            {
                CaliberwebLicense license = service.ValidateLicense(File.ReadAllText(licenseFile));

                log.Info("License:");
                log.Info("-----------------------------");
                log.InfoFormat("Licensee: {0}", license.Licensee);
                log.InfoFormat("Track: {0}", license.HomeTrack);
                log.InfoFormat("Type: {0}", license.Type);
                log.InfoFormat("Expires: {0}", license.Expires);
            }
        }
    }

    [DataContract]
    [Serializable]
    public class CaliberwebLicense : LicenseBase
    {
        public CaliberwebLicense(ILicense license) : base(license)
        {}

        [DataMember]
        public string HomeTrack { get; set; }
    }
}