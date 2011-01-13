using System;
using System.IO;

namespace Caliberweb.Core.Net
{
    public class FileWebClient : IWebClient
    {
        public string Download(Uri uri)
        {
            using(var reader = new StreamReader(uri.LocalPath))
            {
                return reader.ReadToEnd();
            }
        }

        public string Download(Uri uri, DirectoryInfo output)
        {
            return Download(uri);
        }
    }
}