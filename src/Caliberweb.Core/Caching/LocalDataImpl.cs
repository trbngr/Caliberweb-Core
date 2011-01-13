using System;
using System.Collections;

using Caliberweb.Core.Caching.Storage;

namespace Caliberweb.Core.Caching
{
    internal class LocalDataImpl : ILocalData
    {
        private readonly IStorageStrategy strategy;
        private readonly ILocalDataEventHandler eventHandler;
        private readonly Hashtable storage;
        private static readonly object monitor = new object();

        public LocalDataImpl(IStorageStrategy strategy, ILocalDataEventHandler eventHandler)
        {
            this.strategy = strategy;
            this.eventHandler = eventHandler;
            storage = strategy.GetStore();
        }

        public IEnumerator GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        public T Get<T>(object key)
        {
            T value;
            TryGet(key, out value);
            return value;
        }

        public bool TryGet<T>(object key, out T value)
        {
            value = default(T);

            object data = this[key];

            if (data == null)
            {
                eventHandler.OnValueNotFound(strategy, key);
                return false;
            }

            if (data is T)
            {
                value = (T)data;
                eventHandler.OnValueFound(strategy, key, value);
                return true;
            }

            eventHandler.OnValueNotFound(strategy, key);
            return false;
        }

        public void Set<T>(object key, T value)
        {
            this[key] = value;
        }

        public object this[object key]
        {
            get { return Do(() => storage[key]); }
            set
            {
                Do(() =>
                {
                    storage[key] = value;

                    eventHandler.OnValueAdded(strategy, key, value);
                });
            }
        }

        public void Remove(object key)
        {
            Do(() =>
            {
                storage.Remove(key);
                eventHandler.OnValueRemoved(strategy, key);
            });
        }

        public void Remove(params object[] keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public bool Contains(object key)
        {
            return Do(() => storage.ContainsKey(key));
        }

        public void Clear()
        {
            Do(() =>
            {
                storage.Clear();
                eventHandler.OnClear(strategy);
            });
        }

        public int Count
        {
            get { return Do(()=>storage.Count); }
        }

        public string Name
        {
            get { return strategy.BucketName; }
        }

        private T Do<T>(Func<T> function)
        {
            if (strategy.UseSynchLock)
            {
                lock (monitor)
                {
                    return function();
                }
            }

            return function();
        }

        private void Do(Action action)
        {
            if (strategy.UseSynchLock)
            {
                lock (monitor)
                {
                    action();
                }
            }
            else
            {
                action();
            }
        }

        public override string ToString()
        {
            return string.Format("Local Data [{0}]", strategy);
        }
    }
}