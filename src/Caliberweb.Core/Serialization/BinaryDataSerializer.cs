using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Caliberweb.Core.Serialization
{
    internal class BinaryDataSerializer : IDataSerializer
    {
        private readonly BinaryFormatter formatter;

        public BinaryDataSerializer()
        {
            formatter = new BinaryFormatter();
        }

        #region IDataSerializer Members

        public void Serialize<T>(T instance, Stream stream)
        {
            formatter.Serialize(stream, instance);
        }

        public byte[] Serialize<T>(T instance)
        {
            using(var stream = new MemoryStream())
            {
                Serialize(instance, stream);

                return stream.ToArray();
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            return (T) formatter.Deserialize(stream);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using(var stream = new MemoryStream(bytes))
            {
                return Deserialize<T>(stream);
            }
        }

        #endregion
    }
}