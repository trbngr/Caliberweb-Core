using System.Collections;

namespace Caliberweb.Core.Caching.Storage
{
    public interface IStorageStrategy : IEnumerable
    {
        Hashtable GetStore();
        bool UseSynchLock { get; }
        string BucketName { get; }
    }
}