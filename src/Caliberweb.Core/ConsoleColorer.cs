using System;

namespace Caliberweb.Core
{
    public class ConsoleColorer : IDisposable
    {
        private readonly ConsoleColor old;

        public ConsoleColorer(string name)
            : this(GetColorForName(name))
        {}

        public ConsoleColorer(ConsoleColor @new)
        {
            old = Console.ForegroundColor;
            Console.ForegroundColor = @new;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Console.ForegroundColor = old;
        }

        #endregion

        private static ConsoleColor GetColorForName(string name)
        {
            Array values = Enum.GetValues(typeof (ConsoleColor));
            return (ConsoleColor) values.GetValue(Math.Abs(name.GetHashCode())%values.Length);
        }
    }
}