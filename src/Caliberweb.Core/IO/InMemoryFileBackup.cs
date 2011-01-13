using System;
using System.Collections.Generic;
using System.IO;

namespace Caliberweb.Core.IO
{
    public class InMemoryFileBackup : IFileBackup, IDisposable
    {
        private readonly FileInfo file;
        private byte[] bytes;

        public InMemoryFileBackup(FileInfo file)
        {
            this.file = file;

            file.Refresh();

            if (!file.Exists)
            {
                throw new FileNotFoundException("File not found.", file.FullName);
            }

            ((IFileBackup) this).Create();
        }

        public void Dispose()
        {
            file.Refresh();

            File.WriteAllBytes(file.FullName, bytes);
        }

        IEnumerable<IFileBackupInfo> IFileBackup.Create()
        {
            bytes = File.ReadAllBytes(file.FullName);
            return new GenerationalFileBackupInfo[0];
        }
    }
}