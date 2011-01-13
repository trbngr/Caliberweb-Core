using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Caliberweb.Core.Caching.Storage
{
    class HttpCacheStorageStrategy : StorageStrategyBase
    {
        private readonly TimeSpan expiration;
        
        private const CacheItemPriority PRIORITY = CacheItemPriority.Normal;

        public HttpCacheStorageStrategy(Type type, TimeSpan expiration, ILocalDataEventHandler handler) : base(type, handler)
        {
            this.expiration = expiration;
        }

        public override Hashtable GetStore()
        {
            var cache = HttpRuntime.Cache;

            var locker = new DoubleLocker<Hashtable>(
                () => cache[BucketName] as Hashtable,
                () =>
                {
                    var bucket = new Hashtable();

                    cache.Add(BucketName,
                              bucket,
                              null,
                              Cache.NoAbsoluteExpiration,
                              expiration,
                              PRIORITY,
                              ItemRemovedCallback);

                    OnBucketCreation();

                    return bucket;
                });

            return locker.Create();
        }

        private void ItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            HttpRuntime.Cache.Remove(BucketName);
            EventHandler.OnValueRemoved(this, key);
        }

        public override bool UseSynchLock
        {
            get { return true; }
        }
    }
}