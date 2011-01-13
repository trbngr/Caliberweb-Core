using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Caliberweb.Core.Benchmarking.Builders
{
    public interface IBenchmarkSeriesBuilder
    {
        IBenchmarkSeriesBuilder Add(Expression<Action> expression);
        IEnumerable<IBenchmark> AsEnumerable();
    }
}