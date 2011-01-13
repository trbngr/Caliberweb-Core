using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using NUnit.Framework;

namespace Caliberweb.Core.Configuration
{
    [TestFixture]
    public class AppSettingsAccessorTests
    {
        private NameValueCollection collection;
        private ITestConfiguration configuration;

        [SetUp]
        public virtual void SetUp()
        {
            collection = new NameValueCollection
            {
                {TestConfiguration.LIB_KEY, "Caliberweb.Core.Tests"}
            };

            configuration = new TestConfiguration(collection);
        }

        [Test]
        public void will_fall_back_on_default_value_if_key_does_not_exist()
        {
            Version version = configuration.Version;

            Assert.AreEqual(TestConfiguration.DefaultVersion, version);
        }

        [Test]
        public void will_get_correct_value_from_existing_key()
        {
            string name = configuration.LibraryName;

            Assert.AreEqual("Caliberweb.Core.Tests", name);
        }

        [Test]
        public void will_throw_exception_when_key_does_not_exist_and_no_default_value_is_provided()
        {
#pragma warning disable 219
            string author = null;
#pragma warning restore 219
            
            Assert.Throws<KeyNotFoundException>(() => author = configuration.Author);
        }
    }


    public class TestConfiguration : AppSettingsAccessor, ITestConfiguration
    {
        public static readonly Version DefaultVersion;

        public TestConfiguration(NameValueCollection collection) : base(collection)
        {}

        static TestConfiguration()
        {
            DefaultVersion = new Version(1, 0, 0, 15);
        }

        public const string LIB_KEY = "library.name";
        public const string VERSION_KEY = "library.version";
        public const string AUTHOR_KEY = "library.author";

        public Version Version
        {
            get { return GetValue(VERSION_KEY, DefaultVersion); }
        }

        public string LibraryName
        {
            get { return GetValue(LIB_KEY); }
        }

        public string Author
        {
            get { return GetValue(AUTHOR_KEY); }
        }
    }

    public interface ITestConfiguration
    {
        Version Version { get; }
        string LibraryName { get; }
        string Author { get; }
    }

}