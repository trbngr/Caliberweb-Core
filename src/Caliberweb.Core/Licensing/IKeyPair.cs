using System.IO;

using Caliberweb.Core.Serialization;

namespace Caliberweb.Core.Licensing
{
    public interface IKeyPair
    {
        byte[] Public { get; }
        byte[] Private { get; }
        void Save(FileInfo file);
        void Save(FileInfo file, IDataSerializer serializer);
    }
}