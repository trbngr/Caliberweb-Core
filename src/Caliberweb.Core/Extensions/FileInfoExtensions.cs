using System.IO;

namespace Caliberweb.Core.Extensions
{
    public static class FileInfoExtensions
    {
        public static string ReadAllText(this FileInfo file)
        {
            file.Refresh();

            if (!file.Exists)
                return string.Empty;

            return File.ReadAllText(file.FullName);
        }
    }
}