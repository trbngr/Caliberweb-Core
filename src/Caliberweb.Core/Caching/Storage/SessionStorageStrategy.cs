using System;
using System.Collections;
using System.Web;

namespace Caliberweb.Core.Caching.Storage
{
    class SessionStorageStrategy : StorageStrategyBase
    {
        public SessionStorageStrategy(Type type, ILocalDataEventHandler eventHandler)
            : base(type, eventHandler){}

        public override Hashtable GetStore()
        {
            if (!RunningInWebContext)
                throw new InvalidOperationException(
                    "An ASP.Net session must be available when using Session storage. No ASP.Net session was found in the current context.");

            var locker = new DoubleLocker<Hashtable>(
                () => HttpContext.Current.Session[BucketName] as Hashtable,
                () =>
                {
                    var bucket = new Hashtable();
                    HttpContext.Current.Session[BucketName] = bucket;
                    OnBucketCreation();
                    return bucket;
                });

            return locker.Create();
        }

        public override bool UseSynchLock
        {
            get { return true; }
        }
    }
}