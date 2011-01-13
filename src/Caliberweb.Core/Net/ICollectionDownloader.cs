using System;
using System.Collections.Generic;
using System.IO;

namespace Caliberweb.Core.Net
{
    public interface ICollectionDownloader
    {
        void DownloadAll(IEnumerable<Uri> uris, DirectoryInfo output);
        event EventHandler Finished;
        event EventHandler<FileDownloadedEventArgs> FileDownloaded;
    }                                                                 
}                                                                     