using System.Text;

namespace ConsoleDriver.Internal
{
    internal static class StringExtensions
    {
        public static string ToWords(this string s)
        {
            var builder = new StringBuilder();

            foreach (var c in s.ToCharArray())
            {
                if (char.IsUpper(c) && builder.Length > 0)
                {
                    builder.AppendFormat(" {0}", c);
                }
                else
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }
    }
}