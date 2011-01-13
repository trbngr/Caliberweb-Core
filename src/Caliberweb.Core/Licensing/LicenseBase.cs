using System;
using System.Runtime.Serialization;

namespace Caliberweb.Core.Licensing
{
    [DataContract]
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

        [DataMember]
        public string Licensee { get; private set; }

        [DataMember]
        public LicenseType Type { get; private set; }

        [DataMember]
        public DateTime Expires { get; private set; }

        [DataMember]
        public byte[] Signature { get; set; }
    }
}