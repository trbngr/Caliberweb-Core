using System;

namespace Caliberweb.Core
{
    public class DoubleLocker<T> where T : class
    {
        private readonly Func<T> checker;
        private readonly Func<T> creator;
        private static readonly object monitor = new object();

        public DoubleLocker(Func<T> checker, Func<T> creator)
        {
            this.checker = checker;
            this.creator = creator;
        }

        public T Create()
        {
            T value = checker();

            if (value == null)
            {
                lock (monitor)
                {
                    value = checker() ?? creator();
                }
            }

            return value;
        }
    }
}