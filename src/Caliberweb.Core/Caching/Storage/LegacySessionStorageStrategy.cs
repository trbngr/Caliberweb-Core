using System;
using System.Collections;
using System.Web;

namespace Caliberweb.Core.Caching.Storage
{
    class LegacySessionStorageStrategy : StorageStrategyBase
    {
        private Hashtable internalBucket;

        public LegacySessionStorageStrategy(Type type, ILocalDataEventHandler handler) : base(type, handler)
        {}

        public override Hashtable GetStore()
        {
            if (!RunningInWebContext)
                throw new InvalidOperationException(
                    "An ASP.Net session must be available when using Session storage. No ASP.Net session was found in the current context.");

            internalBucket = new Hashtable();

            foreach (object o in HttpContext.Current.Session.Keys)
            {
                internalBucket[o.ToString()] = HttpContext.Current.Session[o.ToString()];
            }

            OnBucketCreation();

            return internalBucket;
        }

        public override bool UseSynchLock
        {
            get { return true; }
        }
    }
}