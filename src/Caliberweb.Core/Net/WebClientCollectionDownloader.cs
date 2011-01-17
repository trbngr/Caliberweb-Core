using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

using MiscUtil.Threading;

namespace Caliberweb.Core.Net
{
    public class WebClientCollectionDownloader : CollectionDownloaderBase
    {
        public override void DownloadAll(IEnumerable<Uri> uris, DirectoryInfo output)
        {
            var poolName = string.Format("downloads to {0}", output.Name);

            var pool = new CustomThreadPool(poolName)
            {
                MinThreads = 1,
                MaxThreads = 4
            };

            pool.AfterWorkItem += ItemCompleted;

            foreach (Uri uri in uris)
            {
                var workItem = new DownloadItem(DownloadOne, uri, output);
                pool.AddWorkItem(workItem);
            }

            while(pool.WorkingThreads > 0)
            {}

            InvokeFinished();
        }

        private void ItemCompleted(CustomThreadPool pool, ThreadPoolWorkItem workitem)
        {
            var item = workitem as DownloadItem;
            if (item == null)
                return;

            InvokeFileDownloaded(item.Uri);
        }

        private static void DownloadOne(Uri uri, FileSystemInfo output)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(uri, GetLocalFilePath(uri, output));
            }
        }

        private class DownloadItem : ThreadPoolWorkItem
        {
            public Uri Uri { get; private set; }

            public DownloadItem(Action<Uri, DirectoryInfo> action, Uri uri, DirectoryInfo output)
                : base(new Action(() => action(uri, output)))
            {
                Uri = uri;
            }
        }
    }
}