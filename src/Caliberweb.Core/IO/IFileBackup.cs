using System.Collections.Generic;

namespace Caliberweb.Core.IO
{
    public interface IFileBackup
    {
        IEnumerable<IFileBackupInfo> Create();
    }
}