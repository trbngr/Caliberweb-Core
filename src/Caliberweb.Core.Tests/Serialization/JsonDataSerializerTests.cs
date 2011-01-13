using System;
using System.Runtime.Serialization;
using System.Text;

using NUnit.Framework;

using System.Linq;

namespace Caliberweb.Core.Serialization
{
    [TestFixture]
    public class JsonDataSerializerTests
    {
        private IDataSerializer serializer;

        private const string JSON = @"{""Email"":""cmartin@crystaltech.com"",""Name"":""cmartin""}";
        private JsonTestObject testObject;

        [SetUp]
        public virtual void SetUp()
        {
            serializer = new JsonDataSerializer();

            testObject = new JsonTestObject("cmartin", "cmartin@crystaltech.com");
        }

        [Test]
        public void can_serialize_object()
        {
            byte[] bytes = serializer.Serialize(testObject);

            string s = Encoding.Default.GetString(bytes);

            Assert.AreEqual(JSON, s);
        }

        [Test]
        public void can_deserialize_from_byte_array()
        {
            var test = new byte[] {123,34,69,109,97,105,108,34,58,34,99,109,97,114,116,105,110,64,99,114,121,115,116,97,108,116,101,99,104,46,99,111,109,34,44,34,78,97,109,101,34,58,34,99,109,97,114,116,105,110,34,125};

            var result = serializer.Deserialize<JsonTestObject>(test);

            Assert.AreEqual(testObject, result);
        }
    }

    [DataContract]
    public class JsonTestObject
    {
        public JsonTestObject(string name, string email)
        {
            Name = name;
            Email = email;
        }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string Email { get; set; }

        public bool Equals(JsonTestObject other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Name, Name) && Equals(other.Email, Email);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (JsonTestObject))
            {
                return false;
            }
            return Equals((JsonTestObject) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Name.GetHashCode()*397) ^ Email.GetHashCode();
            }
        }

        public static bool operator ==(JsonTestObject left, JsonTestObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(JsonTestObject left, JsonTestObject right)
        {
            return !Equals(left, right);
        }
    }
}