namespace Caliberweb.Core.Persistance.Identity
{
    interface IIdentifier<T>
    {
        T Identity { get; }
    }
}