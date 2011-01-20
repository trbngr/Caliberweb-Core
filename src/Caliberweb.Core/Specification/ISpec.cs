using System;

namespace Caliberweb.Core.Specification
{
    public interface ISpec<T>
    {
        bool IsSatisfied(T source);

        ISpec<T> And(ISpec<T> spec);
        ISpec<T> Or(ISpec<T> spec);
        ISpec<T> AndNot(ISpec<T> spec);

        ISpec<T> And(Func<T, bool> spec);
        ISpec<T> Or(Func<T, bool> spec);
        ISpec<T> AndNot(Func<T, bool> spec);

        ISpec<T> Negate();
    }
}