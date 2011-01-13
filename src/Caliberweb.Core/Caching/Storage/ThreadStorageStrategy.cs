using System;
using System.Collections;
using System.Web;

namespace Caliberweb.Core.Caching.Storage
{
    internal class ThreadStorageStrategy : StorageStrategyBase
    {
        [ThreadStatic]
        protected static Hashtable internalBucket;

        public ThreadStorageStrategy(Type type, ILocalDataEventHandler eventHandler)
            : base(type, eventHandler)
        {}

        public override Hashtable GetStore()
        {
            if (RunningInWebContext)
            {
                internalBucket = HttpContext.Current.Items[BucketName] as Hashtable;
                
                if (internalBucket == null)
                {
                    HttpContext.Current.Items[BucketName] = internalBucket = new Hashtable();
                }

                return internalBucket;
            }

            return internalBucket ?? (internalBucket = new Hashtable());
        }
    }
}