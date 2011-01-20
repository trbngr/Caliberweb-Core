using System;

namespace Caliberweb.Core.Specification
{
    public abstract class Spec<T> : ISpec<T>
    {
        #region ISpec<T> Members

        public abstract bool IsSatisfied(T source);

        public ISpec<T> And(ISpec<T> spec)
        {
            return new AndSpec<T>(this, spec);
        }

        public ISpec<T> Or(ISpec<T> spec)
        {
            return new OrSpec<T>(this, spec);
        }

        public ISpec<T> AndNot(ISpec<T> spec)
        {
            return new NotSpec<T>(this, spec);
        }

        public ISpec<T> And(Func<T, bool> spec)
        {
            return new AndSpec<T>(this, Create(spec));
        }

        public ISpec<T> Or(Func<T, bool> spec)
        {
            return new OrSpec<T>(this, Create(spec));
        }

        public ISpec<T> AndNot(Func<T, bool> spec)
        {
            return new NotSpec<T>(this, Create(spec));
        }

        public ISpec<T> Negate()
        {
            return Create(t => !IsSatisfied(t));
        }

        #endregion

        #region Nested type: AndSpec

        private class AndSpec<TSource> : Composite<TSource>
        {
            public AndSpec(ISpec<TSource> left, ISpec<TSource> right)
                : base(left, right)
            {}

            protected override bool SatisfiesComposite(ISpec<TSource> left, ISpec<TSource> right, TSource source)
            {
                return left.IsSatisfied(source) && right.IsSatisfied(source);
            }
        }

        #endregion

        #region Nested type: Composite

        private abstract class Composite<TSource> : Spec<TSource>
        {
            private readonly ISpec<TSource> left;
            private readonly ISpec<TSource> right;

            protected Composite(ISpec<TSource> left, ISpec<TSource> right)
            {
                this.left = left;
                this.right = right;
            }

            public override bool IsSatisfied(TSource source)
            {
                return SatisfiesComposite(left, right, source);
            }

            protected abstract bool SatisfiesComposite(ISpec<TSource> left, ISpec<TSource> right, TSource source);
        }

        #endregion

        #region Nested type: NotSpec

        private class NotSpec<TSource> : Composite<TSource>
        {
            public NotSpec(ISpec<TSource> left, ISpec<TSource> right) : base(left, right)
            {}

            protected override bool SatisfiesComposite(ISpec<TSource> left, ISpec<TSource> right, TSource source)
            {
                return left.IsSatisfied(source) && !right.IsSatisfied(source);
            }
        }

        #endregion

        #region Nested type: OrSpec

        private class OrSpec<TSource> : Composite<TSource>
        {
            public OrSpec(ISpec<TSource> left, ISpec<TSource> right)
                : base(left, right)
            {}

            protected override bool SatisfiesComposite(ISpec<TSource> left, ISpec<TSource> right, TSource source)
            {
                return left.IsSatisfied(source) || right.IsSatisfied(source);
            }
        }

        #endregion

        public static ISpec<T> Create(Func<T, bool> spec)
        {
            return new DynamicSpec<T>(spec);
        }

        public static ISpec<T> Empty
        {
            get { return new DynamicSpec<T>(s => true); }
        }

        #region Nested type: DynamicSpec

        private class DynamicSpec<TSource> : Spec<TSource>
        {
            private readonly Func<TSource, bool> spec;

            public DynamicSpec(Func<TSource, bool> spec)
            {
                this.spec = spec;
            }

            public override bool IsSatisfied(TSource source)
            {
                return spec(source);
            }
        }

        #endregion
    }
}