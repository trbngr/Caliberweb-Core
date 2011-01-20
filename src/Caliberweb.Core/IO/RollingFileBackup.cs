using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Caliberweb.Core.IO
{
    public class RollingFileBackup : IFileBackup
    {
        private readonly FileInfo file;
        private readonly int maxBackups;
        private readonly List<IFileBackupInfo> versions;

        public RollingFileBackup(FileInfo file, int maxBackups)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException("File not found.", file.FullName);
            }

            this.file = file;
            this.maxBackups = maxBackups;
            versions = new List<IFileBackupInfo>();
        }

        #region IFileBackup Members

        public IEnumerable<IFileBackupInfo> Create()
        {
            string searchPattern = string.Format("{0}.*", Path.GetFileNameWithoutExtension(file.Name));

            var files = file.Directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly)
                .OrderByDescending(f => f.Name)
                .ToList();

            files.ForEach(AttemptBackup);

            versions.Sort();
            return versions;
        }

        private void AttemptBackup(FileInfo f)
        {
            var version = new GenerationalFileBackupInfo(f);
            
            if (versions.Contains(version))
                return;

            if(version.CurrentGeneration > maxBackups)
            {
                f.Delete();
                return;
            }

            if (version.NextGeneration > maxBackups)
            {
                return;
            }

            FileInfo destination = version.NextFile;

            if (destination.Exists)
            {
                destination.Delete();
            }

            versions.Add(version);

            File.Copy(f.FullName, destination.FullName);
        }

        #endregion
    }
}