using System;

namespace Caliberweb.Core.Net
{
    [Serializable]
    public class FileDownloadedEventArgs : EventArgs
    {
        public FileDownloadedEventArgs(Uri file)
        {
            File = file;
        }

        public Uri File { get; private set; }
    }
}