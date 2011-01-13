// ReSharper disable SuggestBaseTypeForParameter
using System.IO;

namespace Caliberweb.Core.IO
{
    class DirectoryFileBackupInfo : FileBackupInfoBase
    {
        public DirectoryFileBackupInfo(FileInfo original, DirectoryInfo destination) : base(original)
        {
            NextFile = new FileInfo(Path.Combine(destination.FullName, original.Name));

            IsCurrentlyVersioned = NextFile.Exists;
        }
    }
}
// ReSharper restore SuggestBaseTypeForParameter