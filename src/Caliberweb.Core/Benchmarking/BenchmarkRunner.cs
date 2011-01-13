using System.Collections.Generic;
using System.Linq;

using Caliberweb.Core.Benchmarking.Builders;

namespace Caliberweb.Core.Benchmarking
{
    public class BenchmarkRunner
    {
        private readonly IBenchmarkRenderer renderer;

        public BenchmarkRunner()
            : this(BenchmarkRendering.None)
        {}

        public BenchmarkRunner(IBenchmarkRenderer renderer)
        {
            this.renderer = renderer ?? BenchmarkRendering.None;
        }

        public IEnumerable<IBenchmarkResult> Run(IEnumerable<IBenchmark> benchmarks)
        {
            var results = benchmarks.Select(benchmark =>
            {
                IBenchmarkResult result = benchmark.Run();

                renderer.Render(result);

                return result;
            });

            return results.ToArray();
        }

        public IEnumerable<IBenchmarkResult> Run(IBenchmarkSeriesBuilder builder)
        {
            return Run(builder.AsEnumerable());
        }
    }
}