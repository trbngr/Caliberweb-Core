using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;

using Caliberweb.Core.Reflection;

namespace Caliberweb.Core.Serialization
{
    internal class JsonDataSerializer : IDataSerializer
    {
        #region IDataSerializer Members

        public void Serialize<T>(T instance, Stream stream)
        {
            GetSerializer<T>().WriteObject(stream, instance);
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
            var instance = GetSerializer<T>().ReadObject(stream);

            return (T) instance;
        }

        private static DataContractJsonSerializer GetSerializer<T>()
        {
            Predicate<Assembly> filter = AssemblyProvider.Filters.NonVendor;

            IAssemblyProvider assemblyProvider = AssemblyProvider.FromApplicationBase(filter);

            return new DataContractJsonSerializer(typeof(T), KnownTypes.Of<T>(assemblyProvider));
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