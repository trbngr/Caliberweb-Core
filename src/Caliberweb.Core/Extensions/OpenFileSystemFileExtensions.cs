using System.Collections;
using System.Collections.Generic;
using System.IO;

using OpenFileSystem.IO;

namespace Caliberweb.Core.Extensions
{
    public static class OpenFileSystemFileExtensions
    {
        public static string[] ReadAllLines(this IFile file)
        {
            var list = new ArrayList();

            using (var reader = new StreamReader(file.OpenRead()))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    list.Add(str);
                }
            }

            return (string[]) list.ToArray(typeof (string));
        }

        public static void WriteLine(this IFile file, string line)
        {
            WriteLines(file, new[] {line});
        }

        public static void WriteLines(this IFile file, IEnumerable<string> lines)
        {
            using (var writer = new StreamWriter(file.OpenWrite()))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}