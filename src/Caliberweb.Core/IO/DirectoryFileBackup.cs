using System.Collections.Generic;
using System.IO;

namespace Caliberweb.Core.IO
{
    public class DirectoryFileBackup : IFileBackup
    {
        private readonly FileInfo file;
        private readonly DirectoryInfo destination;

        public DirectoryFileBackup(FileInfo file, DirectoryInfo destination)
        {
            this.file = file;
            this.destination = destination;

            destination.Refresh();
            if(!destination.Exists)
                destination.Create();
        }

        public IEnumerable<IFileBackupInfo> Create()
        {
            var backupInfo = new DirectoryFileBackupInfo(file, destination);

            backupInfo.Refresh();

            var resultingFile = backupInfo.NextFile;

            if (resultingFile.Exists)
            {
                resultingFile.Delete();
            }

            File.Copy(backupInfo.CurrentFile.FullName, backupInfo.NextFile.FullName);

            yield return backupInfo;
        }
    }
}