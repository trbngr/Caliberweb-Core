using Caliberweb.Core.Caching.Storage;

namespace Caliberweb.Core.Caching
{
    public interface ILocalDataEventHandler
    {
        void OnInit(IStorageStrategy sender);
        void OnValueAdded<T>(IStorageStrategy sender, object key, T value);
        void OnValueNotFound(IStorageStrategy sender, object key);
        void OnValueFound<T>(IStorageStrategy sender, object key, T value);
        void OnValueRemoved(IStorageStrategy sender, object key);
        void OnClear(IStorageStrategy sender);
    }
}