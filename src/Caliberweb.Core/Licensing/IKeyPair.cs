using System.IO;

using Caliberweb.Core.Serialization;

namespace Caliberweb.Core.Licensing
{
    public interface IKeyPair
    {
        string Public { get; }
        string Private { get; }
        void Save(FileInfo file, IDataSerializer serializer);
    }
}