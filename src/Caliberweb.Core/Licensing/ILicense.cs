using System;

namespace Caliberweb.Core.Licensing
{
    public interface ILicense
    {
        string Licensee { get; }
        LicenseType Type { get; }
        DateTime Expires { get; }
        byte[] Signature { get; set; }
    }
}