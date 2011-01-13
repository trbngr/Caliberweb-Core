using Caliberweb.Core.Caching.Storage;

namespace Caliberweb.Core.Caching
{
    class NullLocalDataEventHandler : ILocalDataEventHandler
    {
        public void OnInit(IStorageStrategy sender)
        {}

        public void OnValueAdded<T>(IStorageStrategy sender, object key, T value)
        {}

        public void OnValueNotFound(IStorageStrategy sender, object key)
        {}

        public void OnValueFound<T>(IStorageStrategy sender, object key, T value)
        {}

        public void OnValueRemoved(IStorageStrategy sender, object key)
        {}

        public void OnClear(IStorageStrategy sender)
        {}
    }
}