using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Caliberweb.Core.Benchmarking.Builders
{
    internal class BenchmarkBuilder : IBenchmarkBuilder
    {
        private readonly Expression<Action> expression;

        internal BenchmarkBuilder(Expression<Action> expression)
        {
            this.expression = expression;
        }

        #region IBenchmarkBuilder Members

        public IBenchmark Iterate(int iterations)
        {
            return new Benchmark(expression, iterations);
        }

        public IEnumerable<IBenchmark> Series(IEnumerable<int> iterations)
        {
            return new BenchmarkSeriesBuilder(iterations)
                .Add(expression)
                .AsEnumerable();
        }

        #endregion
    }
}