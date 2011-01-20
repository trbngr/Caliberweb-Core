using System.IO;

namespace Caliberweb.Core.IO
{
    public abstract class FileBackupInfoBase : IFileBackupInfo
    {
        protected FileBackupInfoBase(FileInfo original)
        {
            CurrentFile = original;
        }

        public bool IsCurrentlyVersioned { get; protected set; }
        public FileInfo CurrentFile { get; private set; }
        public FileInfo NextFile { get; protected set; }

        public void Refresh()
        {
            CurrentFile.Refresh();
            NextFile.Refresh();
        }

        public bool Equals(IFileBackupInfo other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.CurrentFile.FullName, CurrentFile.FullName) && Equals(other.NextFile.FullName, NextFile.FullName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType().IsAssignableFrom(typeof(IFileBackupInfo)))
            {
                return false;
            }
            return Equals((IFileBackupInfo)obj);
        }

        public override int GetHashCode()
        {
            return CurrentFile.FullName.GetHashCode();
        }

        public int CompareTo(IFileBackupInfo other)
        {
            return CurrentFile.FullName.CompareTo(other.CurrentFile.FullName);
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", CurrentFile.Name, NextFile.Name);
        }
    }
}