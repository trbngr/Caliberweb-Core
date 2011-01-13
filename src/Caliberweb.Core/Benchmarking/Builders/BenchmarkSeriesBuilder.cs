using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Caliberweb.Core.Benchmarking.Builders
{
    class BenchmarkSeriesBuilder : IBenchmarkSeriesBuilder
    {
        private readonly IEnumerable<int> iterations;
        private readonly List<IBenchmark> benchmarks;

        public BenchmarkSeriesBuilder(IEnumerable<int> iterations)
        {
            this.iterations = iterations;
            benchmarks = new List<IBenchmark>();
        }

        public IBenchmarkSeriesBuilder Add(Expression<Action> expression)
        {
            foreach (var iteration in iterations)
            {
                benchmarks.Add(Benchmark.Create(expression).Iterate(iteration));
            }

            return this;
        }

        public IEnumerable<IBenchmark> AsEnumerable()
        {
            return benchmarks;
        }
    }
}