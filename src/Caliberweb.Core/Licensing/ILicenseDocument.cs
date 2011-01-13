using System.Xml;

namespace Caliberweb.Core.Licensing
{
    public interface ILicenseDocument
    {
        void AppendData(string name, string text);
    }
}