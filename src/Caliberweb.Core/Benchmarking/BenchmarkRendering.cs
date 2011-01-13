using System;

namespace Caliberweb.Core.Benchmarking
{
    public static class BenchmarkRendering
    {
        public static readonly IBenchmarkRenderer None = new DynamicRenderer(r => { });

        public static IBenchmarkRenderer Set(Action<IBenchmarkResult> renderAction)
        {
            return new DynamicRenderer(renderAction);
        }

        private class DynamicRenderer : IBenchmarkRenderer
        {
            private readonly Action<IBenchmarkResult> renderAction;

            public DynamicRenderer(Action<IBenchmarkResult> renderAction)
            {
                this.renderAction = renderAction;
            }

            public void Render(IBenchmarkResult result)
            {
                renderAction(result);
            }
        }
    }
}