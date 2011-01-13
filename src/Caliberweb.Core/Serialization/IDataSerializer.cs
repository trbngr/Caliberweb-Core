using System.IO;

namespace Caliberweb.Core.Serialization
{
    /// <summary>
    /// Generic data serializer interface.
    /// </summary>
    public interface IDataSerializer
    {
        /// <summary>
        /// Serializes the object to the specified stream
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="stream">The destination stream.</param>
        void Serialize<T>(T instance, Stream stream);

        /// <summary>
        /// Serializes the object to a byte array.
        /// </summary>
        /// <param name="instance">The instance.</param>
        byte[] Serialize<T>(T instance);

        T Deserialize<T>(Stream stream);

        T Deserialize<T>(byte[] bytes);
    }
}