using System;
using System.IO;
using System.Runtime.Serialization;

using Caliberweb.Core.Serialization;

namespace Caliberweb.Core.Licensing
{
    [DataContract]
    [Serializable]
    public class KeyPair : IKeyPair
    {
        private static readonly IDataSerializer defaultSerializer = Serializers.Json;

        internal KeyPair()
        {}

        [DataMember]
        public byte[] Public { get; internal set; }
        
        [DataMember]
        public byte[] Private { get; internal set; }
        
        public static IKeyPair Load(FileInfo file)
        {
            return Load(file, defaultSerializer);
        }

        public static IKeyPair Load(FileInfo file, IDataSerializer serializer)
        {
            file.Refresh();
            if (!file.Exists)
                throw new FileNotFoundException("not found", file.FullName);

            return serializer.Deserialize<KeyPair>(File.ReadAllBytes(file.FullName));
        }

        public void Save(FileInfo file)
        {
            Save(file, defaultSerializer);
        }

        public void Save(FileInfo file, IDataSerializer serializer)
        {
            File.WriteAllBytes(file.FullName, serializer.Serialize(this));
        }
    }


}