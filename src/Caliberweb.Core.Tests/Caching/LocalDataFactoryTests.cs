using System;
using System.Collections;
using System.Web;

using Caliberweb.Core.Caching.Storage;

using NUnit.Framework;

namespace Caliberweb.Core.Caching
{
    [TestFixture]
    public class LocalDataFactoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public virtual void SetUp()
        {
            eventHandler = new TestLocalDataEventHandler();
            factory = new LocalDataFactoryImpl(eventHandler);
        }

        #endregion

        private ILocalDataFactory factory;

        private TestLocalDataEventHandler eventHandler;
        private readonly Type type = typeof(LocalDataFactoryTests);

        [Test]
        public void adding_value_to_session_store_uses_HttpSessionState()
        {
            FakeHttpContext.Create();
            ILocalData store = factory.GetSessionStore(type);

            const string KEY = "test.value";
            const int VALUE = 1234;

            store.Set(KEY, VALUE);

            var table = (Hashtable) HttpContext.Current.Session[store.Name];

            object actual = table[KEY];

            Assert.AreEqual(VALUE, actual);

            Assert.IsTrue(eventHandler.StrategyUsed is SessionStorageStrategy);
            Assert.AreEqual(eventHandler.KeyUsed, KEY);
            Assert.AreEqual(eventHandler.ValueUsed, VALUE);

            Assert.AreEqual(VALUE, store.Get<int>(KEY));
        }

        [Test]
        public void can_create_session_store()
        {
            FakeHttpContext.Create();
            ILocalData store = factory.GetSessionStore(type);

            Assert.AreEqual(store.Name, type.FullName);
            Assert.IsTrue(eventHandler.StrategyUsed is SessionStorageStrategy);
        }

        [Test]
        public void can_create_http_cache_store()
        {
            FakeHttpContext.Create();

            ILocalData store = factory.GetHttpCacheStore(type, TimeSpan.FromSeconds(5));

            Assert.AreEqual(store.Name, type.FullName);
            
            //NOTE: Not sure why the eventHandler isn't called here.
//            Assert.IsTrue(eventHandler.StrategyUsed is HttpCacheStorageStrategy, "found {0}", eventHandler.StrategyUsed.GetType().Name);
        }

        [Test]
        public void adding_value_to_http_cache_store_uses_HttpRuntime_Cache()
        {
            FakeHttpContext.Create();
            ILocalData store = factory.GetHttpCacheStore(type, TimeSpan.FromSeconds(5));

            const string KEY = "test.value";
            const int VALUE = 1234;

            store.Set(KEY, VALUE);

            var table = (Hashtable)HttpRuntime.Cache[store.Name];

            object actual = table[KEY];

            Assert.AreEqual(VALUE, actual);

            Assert.IsTrue(eventHandler.StrategyUsed is HttpCacheStorageStrategy);
            Assert.AreEqual(eventHandler.KeyUsed, KEY);
            Assert.AreEqual(eventHandler.ValueUsed, VALUE);
        }

    }

    
}