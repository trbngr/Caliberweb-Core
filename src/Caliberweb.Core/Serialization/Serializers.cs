namespace Caliberweb.Core.Serialization
{
    public static class Serializers
    {
        public static readonly IDataSerializer Xml = new XmlDataSerializer();
        public static readonly IDataSerializer Binary = new BinaryDataSerializer();
        public static readonly IDataSerializer Json = new JsonDataSerializer();
    }
}