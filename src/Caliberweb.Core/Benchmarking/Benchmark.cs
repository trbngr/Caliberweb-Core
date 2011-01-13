using System;
using System.Linq.Expressions;
using System.Text;

using Caliberweb.Core.Benchmarking.Builders;
using Caliberweb.Core.Extensions;

namespace Caliberweb.Core.Benchmarking
{
    class Benchmark : IBenchmark
    {
        private readonly Expression<Action> expression;
        private readonly int iterations;

        internal Benchmark(Expression<Action> expression, int iterations)
        {
            this.expression = expression;
            this.iterations = iterations;
        }

        #region IBenchmark Members

        public IBenchmarkResult Run()
        {
            Action action = expression.Compile();

            Action test = CreateIteratedTest(action, iterations);

            return new BenchmarkResult(expression, iterations, test.Time());
        }

        #endregion

        public static IBenchmarkSeriesBuilder Series(params int[] iterations)
        {
            return new BenchmarkSeriesBuilder(iterations);
        }

        public static IBenchmarkBuilder Create(Expression<Action> expression)
        {
            return new BenchmarkBuilder(expression);
        }

        private static Action CreateIteratedTest(Action action, int iterations)
        {
            return () =>
            {
                for (int i = 0; i < iterations; i++)
                {
                    action();
                }
            };
        }

        public override string ToString()
        {
            var method = expression.Body as MethodCallExpression;
            if (method == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder(2);

            builder.AppendLine(string.Format("{0} [{1}]", method.Method.Name.ToWords(), iterations));
            builder.AppendLine(expression.ToString());

            return builder.ToString();
        }
    }
}