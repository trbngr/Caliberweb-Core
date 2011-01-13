using System;
using System.IO;
using System.Text;

namespace Caliberweb.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToWords(this string s)
        {
            var builder = new StringBuilder();

            foreach (char c in s)
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

        public static string ToLocalFilePath(this string filename)
        {
            return ToLocalFilePath(filename, false);
        }

        public static string ToLocalFilePath(this string filename, bool createDirectory)
        {
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

            if (createDirectory)
            {
                CreateDirectory(fullPath);
            }

            return fullPath;
        }

        public static FileInfo ToLocalFileInfo(this string filename)
        {
            return new FileInfo(ToLocalFilePath(filename, false));
        }

        public static FileInfo ToLocalFileInfo(this string filename, bool createDirectory)
        {
            return new FileInfo(ToLocalFilePath(filename, createDirectory));
        }

        private static void CreateDirectory(string fullPath)
        {
            string directoryName = Path.GetDirectoryName(fullPath);
            if (directoryName != null)
            {
                Directory.CreateDirectory(directoryName);
            }
        }
    }
}