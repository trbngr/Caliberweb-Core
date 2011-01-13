using System.Collections.Generic;

namespace Caliberweb.Core.Persistance.Identity
{
    internal class IdentityMap<T, TIdentity> : IIdentityMap<T, TIdentity> where T : class, IIdentifier<TIdentity>
    {
        private readonly IDictionary<TIdentity, T> map;
        private static readonly object syncLock = new object();

        public IdentityMap()
        {
            map = new Dictionary<TIdentity, T>();
        }

        public T GetById(TIdentity id)
        {
            lock (syncLock)
            {
                return map.ContainsKey(id) ? map[id] : default(T);
            }
        }

        public void Add(T instance)
        {
            lock (syncLock)
            {
                if (map.ContainsKey(instance.Identity))
                {
                    return;
                }

                map.Add(instance.Identity, instance);
            }
        }

        public void Remove(T instance)
        {
            lock (syncLock)
            {
                if (map.ContainsKey(instance.Identity))
                {
                    map.Remove(instance.Identity);
                }
            }
        }

        public bool TryGetById(TIdentity id, out T instance)
        {
            lock (syncLock)
            {
                instance = GetById(id);
                return instance != default(T);
            }
        }
    }
}