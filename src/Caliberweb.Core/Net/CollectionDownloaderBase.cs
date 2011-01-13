using System;
using System.Collections.Generic;
using System.IO;

namespace Caliberweb.Core.Net
{
    public abstract class CollectionDownloaderBase : ICollectionDownloader
    {
        #region ICollectionDownloader Members

        public abstract void DownloadAll(IEnumerable<Uri> uris, DirectoryInfo output);
        public event EventHandler Finished;
        public event EventHandler<FileDownloadedEventArgs> FileDownloaded;

        #endregion

        protected static string GetLocalFilePath(Uri uri, FileSystemInfo output)
        {
            string name = new FileInfo(uri.LocalPath).Name;

            return Path.Combine(output.FullName, name);
        }

        protected void InvokeFileDownloaded(Uri file)
        {
            EventHandler<FileDownloadedEventArgs> handler = FileDownloaded;
            if (handler != null)
            {
                handler(this, new FileDownloadedEventArgs(file));
            }
        }

        protected void InvokeFinished()
        {
            EventHandler handler = Finished;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}