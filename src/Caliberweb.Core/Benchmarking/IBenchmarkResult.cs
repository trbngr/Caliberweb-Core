using System;

namespace Caliberweb.Core.Benchmarking
{
    public interface IBenchmarkResult
    {
        int Iterations { get; }
        TimeSpan ElapsedTime { get; }
        string Code { get; }
        string Name { get; }
    }
}