using System;
using System.Collections.Generic;

namespace Caliberweb.Core.Extensions
{   
    public static class FuncExtensions
    {
        private static Func<T, TR> CastByExample<T, TR>(Func<T, TR> f, T t)
        {
            return f;
        }

        public static Func<TResult> Memoize<TResult>(this Func<TResult> function)
        {
            TResult result = default(TResult);
            bool inMemory = false;

            return delegate
            {
                if (!inMemory)
                {
                    result = function();
                    inMemory = true;
                }

                return result;
            };
        }

        public static Func<TArg, TResult> Memoize<TArg, TResult>(this Func<TArg, TResult> function)
        {
            var map = new Dictionary<TArg, TResult>();

            return a =>
            {
                TResult result;

                if (!map.TryGetValue(a, out result))
                {
                    map.Add(a, result = function(a));
                }

                return result;
            };
        }

        public static Func<TArg1, TArg2, TResult> Memoize<TArg1, TArg2, TResult>(this Func<TArg1, TArg2, TResult> f)
        {
            var example = new
            {
                A = default(TArg1),
                B = default(TArg2)
            };

            var tuplified = CastByExample(t => f(t.A, t.B), example);

            var memoized = tuplified.Memoize();

            return (a, b) => memoized(new
            {
                A = a,
                B = b
            });
        }
    }
}