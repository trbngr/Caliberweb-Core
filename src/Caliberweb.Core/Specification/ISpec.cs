namespace Caliberweb.Core.Specification
{
    public interface ISpec<T>
    {
        bool IsSatisfied(T source);

        ISpec<T> And(ISpec<T> spec);
        ISpec<T> Or(ISpec<T> spec);
        ISpec<T> Not(ISpec<T> spec);
    }
}