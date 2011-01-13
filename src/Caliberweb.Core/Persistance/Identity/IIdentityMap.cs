namespace Caliberweb.Core.Persistance.Identity
{
    internal interface IIdentityMap<T, TIdentity> where T : class, IIdentifier<TIdentity>
    {
        T GetById(TIdentity id);
        void Add(T instance);
        void Remove(T instance);
        bool TryGetById(TIdentity id, out T instance);
    }
}