namespace Caliberweb.Core.Licensing
{
    public interface ILicenseCreator<T> where T : ILicense
    {
        T CreateLicense(ILicense license);
        void AlterDocument(ILicenseDocument document, T license);
    }
}