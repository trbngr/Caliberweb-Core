using System;
using System.Collections;
using System.Web;

namespace Caliberweb.Core.Caching.Storage
{
    public abstract class StorageStrategyBase : IStorageStrategy
    {
        protected StorageStrategyBase(Type type, ILocalDataEventHandler handler)
        {
            EventHandler = handler ?? new NullLocalDataEventHandler();
            BucketName = type.FullName;
        }

        public IEnumerator GetEnumerator()
        {
            return GetStore().GetEnumerator();
        }

        public abstract Hashtable GetStore();

        public virtual bool UseSynchLock
        {
            get { return false; }
        }

        protected bool RunningInWebContext
        {
            get { return HttpContext.Current != null; }
        }

        public string BucketName { get; private set; }

        protected ILocalDataEventHandler EventHandler { get; private set; }

        protected void OnBucketCreation()
        {
            EventHandler.OnInit(this);
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]", GetType().Name, BucketName);
        }
    }
}