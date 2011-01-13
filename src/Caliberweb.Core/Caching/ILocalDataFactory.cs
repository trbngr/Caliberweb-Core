using System;

using Caliberweb.Core.Caching.Storage;

namespace Caliberweb.Core.Caching
{
    public interface ILocalDataFactory
    {
        ILocalData GetCustomStore(IStorageStrategy strategy);
        ILocalData GetThreadStore(Type type);
        ILocalData GetSessionStore(Type type);
        ILocalData GetHttpCacheStore(Type type, TimeSpan expiration);
    }
}