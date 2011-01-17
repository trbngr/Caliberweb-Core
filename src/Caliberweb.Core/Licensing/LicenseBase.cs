using System;
using System.Runtime.Serialization;

namespace Caliberweb.Core.Licensing
{
    [DataContract(Name = "lic")]
    [Serializable]
    public class LicenseBase : ILicense
    {
        internal LicenseBase(string licensee, LicenseType type, DateTime expires)
        {
            Licensee = licensee;
            Type = type;
            Expires = expires;
        }

        public LicenseBase(ILicense license) : this(license.Licensee, license.Type, license.Expires)
        {}

        [DataMember(Name = "name")]
        public string Licensee { get; private set; }

        [DataMember(Name = "type")]
        public LicenseType Type { get; private set; }

        [DataMember(Name = "expire")]
        public DateTime Expires { get; private set; }

        [DataMember(Name = "sig")]
        public byte[] Signature { get; set; }
    }
}