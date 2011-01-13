using System;
using System.Diagnostics;

namespace Caliberweb.Core.Benchmarking
{
    public static class ActionExtensions
    {
        public static TimeSpan Time(this Action action)
        {
            var watch = new Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            return watch.Elapsed;
        }

        public static TimeSpan Time<T1>(this Action<T1> action, T1 t1)
        {
            var watch = new Stopwatch();
            watch.Start();
            action(t1);
            watch.Stop();
            return watch.Elapsed;
        }
    }
}