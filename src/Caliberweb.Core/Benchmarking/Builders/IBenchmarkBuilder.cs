using System.Collections.Generic;

namespace Caliberweb.Core.Benchmarking.Builders
{
    public interface IBenchmarkBuilder
    {
        IBenchmark Iterate(int iterations);
        IEnumerable<IBenchmark> Series(IEnumerable<int> iterations);
    }
}