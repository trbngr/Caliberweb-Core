using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

using Caliberweb.Core.Reflection;

namespace Caliberweb.Core.Serialization
{
    internal class XmlDataSerializer : IDataSerializer
    {
        public void Serialize<T>(T instance, Stream stream)
        {
            var serializer = GetSerializer<T>();
            serializer.WriteObject(stream, instance);
        }

        private static DataContractSerializer GetSerializer<T>()
        {
            Predicate<Assembly> filter = AssemblyProvider.Filters.NonVendor;

            IAssemblyProvider assemblyProvider = AssemblyProvider.FromApplicationBase(filter);

            return new DataContractSerializer(typeof(T), KnownTypes.Of<T>(assemblyProvider));
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
            var serializer = GetSerializer<T>();
            return (T) serializer.ReadObject(stream);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return Deserialize<T>(stream);
            }
        }
    }
}