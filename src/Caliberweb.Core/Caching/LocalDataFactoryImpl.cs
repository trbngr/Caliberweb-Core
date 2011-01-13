using System;

using Caliberweb.Core.Caching.Storage;

namespace Caliberweb.Core.Caching
{
    public class LocalDataFactoryImpl : ILocalDataFactory
    {
        private readonly ILocalDataEventHandler handler;

        public LocalDataFactoryImpl(ILocalDataEventHandler handler)
        {
            this.handler = handler;
        }

        public ILocalData GetCustomStore(IStorageStrategy strategy)
        {
            return GetLocalData(strategy);
        }

        public ILocalData GetThreadStore(Type type)
        {
            return GetLocalData(new ThreadStorageStrategy(type, handler));
        }

        public ILocalData GetSessionStore(Type type)
        {
            return GetLocalData(new SessionStorageStrategy(type, handler));
        }

        public ILocalData GetHttpCacheStore(Type type, TimeSpan expiration)
        {
            return GetLocalData(new HttpCacheStorageStrategy(type, expiration, handler));
        }

        private ILocalData GetLocalData(IStorageStrategy strategy)
        {
            return new LocalDataImpl(strategy, handler);
        }
    }
}