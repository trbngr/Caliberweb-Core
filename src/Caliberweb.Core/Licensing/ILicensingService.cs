using System;

namespace Caliberweb.Core.Licensing
{
    public interface ILicensingService<T> where T : ILicense
    {
        string GenerateLicense(string licensee, LicenseType type, DateTime expiration);
        T ValidateLicense(string licensePath);
    }
}