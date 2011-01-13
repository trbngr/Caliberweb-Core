using System;
using System.Collections.Generic;
using System.Linq;

namespace Caliberweb.Core
{
    public class ConsoleReader
    {
        public ConsoleReader()
        {
            Console.Clear();
        }

        public int GetInt(string prompt)
        {
            return GetValue(prompt, s => int.Parse(s));
        }

        public string GetString(string prompt)
        {
            Console.Out.Write("Enter {0}: ", prompt);
            var line = Console.ReadLine();
            return line;
        }

        public T GetValueOf<T>(string prompt, IEnumerable<T> list, Func<T, string> formatter)
        {
            Console.Out.WriteLine("{0}: ", prompt);
            int i = 0;
            foreach (T item in list)
            {
                Console.Out.WriteLine("\t{0:00}: {1}", ++i, formatter(item));
            }
            string line = Console.ReadLine();

            if (int.TryParse(line, out i) && i > 0 && i <= list.Count())
            {
                var value = list.ElementAt(--i);
                return value;
            }

            throw new InvalidOperationException("invalid choice");
        }

        public T GetValue<T>(string prompt, Func<string, T> convertor)
        {
            Console.Out.Write("Enter {0}: ", prompt);
            string line = Console.ReadLine();

            try
            {
                var value = convertor(line);

                return value;
            }
            catch
            {
                throw new Exception(string.Format("Invalid {0}", prompt));
            }
        }

        public List<T> GetList<T>(Func<string, T> convert)
        {
            string line = Console.ReadLine();
            var items = new List<T>();

            if (line != null)
            {
                string[] strings = line.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

                items.AddRange(strings.Select(convert));
            }

            return items;
        }

        public bool Confirm(string message)
        {
            Console.Out.Write("{0} [Y/n]: ", message);
            
            var c = Console.ReadKey().KeyChar;
            
            if (c.Equals('y') || c.Equals('Y') || c.Equals('\r'))
            {
                Console.Out.WriteLine("");
                return true;
            }

            if(c.Equals('n') || c.Equals('N'))
            {
                Console.Out.WriteLine("");
                return false;
            }

            Console.Out.WriteLine("invalid choice...");
            return Confirm(message);
        }
    }
}