using System;
using System.IO;
using System.Net;

namespace Caliberweb.Core.Net
{
    public class SystemWebClient : IWebClient
    {
        public string Download(Uri uri)
        {
            using (var client = new WebClient())
            {
                return client.DownloadString(uri);
            }
        }

        public string Download(Uri uri, DirectoryInfo output)
        {
            using (var client = new WebClient())
            {
                string fileName = Path.Combine(output.FullName, Path.GetFileName(uri.LocalPath));

                client.DownloadFile(uri, fileName);

                using (var reader = new StreamReader(fileName))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}