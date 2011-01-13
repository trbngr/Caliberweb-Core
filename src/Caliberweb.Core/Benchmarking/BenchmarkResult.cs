using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Caliberweb.Core.Extensions;

namespace Caliberweb.Core.Benchmarking
{
    class BenchmarkResult : IBenchmarkResult
    {
        public static readonly IEqualityComparer<BenchmarkResult> NameComparer = new NameEqualityComparer();

        internal BenchmarkResult(Expression<Action> expression, int iterations, TimeSpan elapsedTime)
        {
            SetName(expression);

            Iterations = iterations;
            ElapsedTime = elapsedTime;
            Code = expression.ToString();
        }

        private void SetName(Expression<Action> expression)
        {
            var method = expression.Body as MethodCallExpression;
            if (method == null)
            {
                return;
            }

            Name = method.Method.Name.ToWords();
        }

        public int Iterations { get; private set; }
        public TimeSpan ElapsedTime { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }

        private class NameEqualityComparer : IEqualityComparer<BenchmarkResult>
        {
            public bool Equals(BenchmarkResult x, BenchmarkResult y)
            {
                return x.Name.Equals(y.Name);
            }

            public int GetHashCode(BenchmarkResult result)
            {
                return result.Name.GetHashCode();
            }
        }
    }
}