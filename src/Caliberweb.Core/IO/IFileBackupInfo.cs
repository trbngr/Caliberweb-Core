using System;
using System.IO;

namespace Caliberweb.Core.IO
{
    public interface IFileBackupInfo : IComparable<IFileBackupInfo>
    {
        void Refresh();
        bool IsCurrentlyVersioned { get; }
        FileInfo CurrentFile { get; }
        FileInfo NextFile { get; }
    }
}