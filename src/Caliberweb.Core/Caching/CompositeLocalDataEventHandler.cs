using System.Collections.Generic;

using Caliberweb.Core.Caching.Storage;

namespace Caliberweb.Core.Caching
{
    class CompositeLocalDataEventHandler : ILocalDataEventHandler
    {
        private readonly List<ILocalDataEventHandler> handlers;

        public CompositeLocalDataEventHandler(IEnumerable<ILocalDataEventHandler> eventHandlers)
        {
            handlers = new List<ILocalDataEventHandler>(eventHandlers);
        }

        public void AddHandler(ILocalDataEventHandler handler)
        {
            handlers.Add(handler);
        }

        public void OnInit(IStorageStrategy sender)
        {
            handlers.ForEach(h => h.OnInit(sender));
        }

        public void OnValueAdded<T>(IStorageStrategy sender, object key, T value)
        {
            handlers.ForEach(h => h.OnValueAdded(sender, key, value));
        }

        public void OnValueNotFound(IStorageStrategy sender, object key)
        {
            handlers.ForEach(h => h.OnValueNotFound(sender, key));
        }

        public void OnValueFound<T>(IStorageStrategy sender, object key, T value)
        {
            handlers.ForEach(h => h.OnValueFound(sender, key, value));
        }

        public void OnValueRemoved(IStorageStrategy sender, object key)
        {
            handlers.ForEach(h => h.OnValueRemoved(sender, key));
        }

        public void OnClear(IStorageStrategy sender)
        {
            handlers.ForEach(h => h.OnClear(sender));
        }
    }
}