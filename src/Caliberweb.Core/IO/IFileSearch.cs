using System.Collections.Generic;
using System.IO;

namespace Caliberweb.Core.IO
{
    public class FileSearch
    {
        private readonly string searchPattern;

        public FileSearch(string searchPattern)
        {
            this.searchPattern = searchPattern;
        }

        public IEnumerable<FileInfo> PerformSearch(DirectoryInfo path, bool recurse)
        {
            var option = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            
            return path.GetFiles(searchPattern,option);
        }
    }
}