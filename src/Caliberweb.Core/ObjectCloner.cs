using Caliberweb.Core.Serialization;

namespace Caliberweb.Core
{
    public class ObjectCloner : IObjectCloner
    {
        private readonly IDataSerializer serializer;

        public ObjectCloner(IDataSerializer serializer)
        {
            this.serializer = serializer;
        }

        public T Clone<T>(T instance)
        {
            var bytes = serializer.Serialize(instance);
            return serializer.Deserialize<T>(bytes);
        }
    }
}