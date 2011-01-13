using System;
using System.IO;

namespace Caliberweb.Core.Net
{
    public interface IWebClient
    {
        string Download(Uri uri);
        string Download(Uri uri, DirectoryInfo output);
    }
}