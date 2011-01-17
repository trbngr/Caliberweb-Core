using System.Runtime.Serialization;

namespace Caliberweb.Core.Licensing
{
    [DataContract(Name = "lic")]
    public class MyTestLicense : LicenseBase
    {
        public MyTestLicense(ILicense license) : base(license)
        {}

        [DataMember(Name = "street_number")]
        public int StreetNumber { get; set; }

        [DataMember(Name = "street_name")]
        public string StreetName { get; set; }
    }
}